// =======================
// DOM references
// =======================

const titleInput = document.getElementById("movie-title-en") as HTMLInputElement;
const desctiptionInput = document.getElementById("movie-description") as HTMLInputElement;
const yearInput = document.getElementById("release-year") as HTMLInputElement;

const checkboxes = document.querySelectorAll(
    ".checkbox-container input[type='checkbox']"
) as NodeListOf<HTMLInputElement>;

const clearButton = document.getElementById("clear") as HTMLButtonElement;
const addButton = document.getElementById("add") as HTMLButtonElement;

const toast = document.getElementById("toast");
const toastTitle = toast?.querySelector(".toast-title");
const toastMessage = toast?.querySelector(".toast-message");
const toastCloseButton = document.getElementById("toast-close") as HTMLButtonElement;

const modal = document.getElementById("api-modal") as HTMLDivElement;
const apiInput = document.getElementById("api-key") as HTMLInputElement;
const authorizeBtn = document.getElementById("authorize-btn") as HTMLButtonElement;
const modalMessage = document.getElementById("modal-message") as HTMLParagraphElement;

const splashModal = document.getElementById("splash-modal") as HTMLDivElement;
const splashTitle = document.getElementById("splash-title") as HTMLHeadingElement;
const splashMessage = document.getElementById("splash-message") as HTMLParagraphElement;

// =======================
// State
// =======================

let isAuthorized = false;
let toastTimeout: number | undefined;

const apiBase = "https://localhost:7185/api";

// =======================
// Event wiring
// =======================

clearButton.addEventListener("click", clearInput);
addButton.addEventListener("click", sendToAnytype);
toastCloseButton.addEventListener("click", hideToast);

[titleInput, desctiptionInput, yearInput].forEach(input => {
    input.addEventListener("keydown", e => {
        if (e.key === "Enter") {
            e.preventDefault();
            sendToAnytype();
        }
    });
});

authorizeBtn.addEventListener("click", onAuthorizeClick);

// =======================
// Core UI actions
// =======================

function sendToAnytype() {
    if (!isAuthorized) {
        showModal("API key required");
        return;
    }

    const title = titleInput.value;
    const year = yearInput.value;

    if (!isTitleValid(title)) {
        showToast("Ошибка", "Необходимо ввести тайтл");
        titleInput.focus();
        return;
    }

    if (!isYearValid(year)) {
        showToast("Ошибка", "Год должен состоять из 4 цифр");
        yearInput.focus();
        yearInput.select();
        return;
    }

    const categories = Array.from(checkboxes)
        .filter(cb => cb.checked)
        .map(cb => cb.value);

    const movieData = {
        titleEn: title,
        description: desctiptionInput.value,
        releaseYear: year,
        categories
    };

    console.log(movieData);

    clearInput();
    titleInput.focus();
}

function clearInput() {
    titleInput.value = "";
    desctiptionInput.value = "";
    yearInput.value = "";
    checkboxes.forEach(cb => (cb.checked = false));
}

// =======================
// Validation
// =======================

function isTitleValid(title: string): boolean {
    return title.trim().length > 0;
}

function isYearValid(year: string): boolean {
    if (year.trim() === "") {
        return true;
    }

    return /^\d{4}$/.test(year);
}

// =======================
// Toast
// =======================

function showToast(title: string, message: string) {
    if (!(toast instanceof HTMLElement)) {
        throw new Error("Toast element not found");
    }

    toastTitle && (toastTitle.textContent = title);
    toastMessage && (toastMessage.textContent = message);

    toast.style.transform = "translateX(0)";

    if (toastTimeout) {
        clearTimeout(toastTimeout);
    }

    toastTimeout = window.setTimeout(hideToast, 3500);
}

function hideToast() {
    if (!(toast instanceof HTMLElement)) {
        throw new Error("Toast element not found");
    }
    toast.style.transform = "translateX(110%)";
}

// =======================
// Modal
// =======================

function showModal(message?: string) {
    modal.style.display = "flex";
    modalMessage.textContent = message ?? "";
}

function hideModal() {
    modal.style.display = "none";
    modalMessage.textContent = "";
}

function showSplash(title: string, message: string) {
    splashTitle.textContent = title;
    splashMessage.textContent = message;
    splashModal.style.display = "flex";
}

function hideSplash() {
    splashModal.style.display = "none";
}

// =======================
// Backend API
// =======================

async function checkLive(): Promise<boolean> {
    console.log("Runnung checkLive method before try");
    
    try {
        const res = await fetch(`${apiBase}/health/live`);
        console.log("Runnung checkLive method!");    
        return res.ok;
    } catch {
        return false;
    }
}

async function checkReady(): Promise<boolean> {
    try {
        const res = await fetch(`${apiBase}/health/ready`);

        return res.ok;
    } catch {
        return false;
    }
}

async function authorize(apiKey: string): Promise<"ok" | "rejected" | "network"> {
    try {
        const res = await fetch(`${apiBase}/auth/authorize`, {
            method: "POST",
            headers: {
                Authorization: `Bearer ${apiKey}`
            }
        });

        return res.ok ? "ok" : "rejected";
    } catch {
        return "network";
    }
}

// =======================
// Authorization flow
// =======================

async function onAuthorizeClick() {
    const key = apiInput.value.trim();
    if (!key) {
        return;
    }

    const result = await authorize(key);

    if (result === "network") {
        showModal("Backend is not running");
        return;
    }

    if (result === "rejected") {
        showModal("Invalid API key");
        return;
    }

    const ready = await checkReady();
    ready ? setAuthorized() : setUnauthorized("API key required");
}

function setAuthorized() {
    isAuthorized = true;
    hideModal();
}

function setUnauthorized(message: string) {
    isAuthorized = false;
    showModal(message);
}

// =======================
// Startup
// =======================
async function init() {
    showSplash("Starting application", "Checking backend status…");

    // Poll until backend is live
    while (!(await checkLive())) {
        showSplash("Backend is not running", "Please start the server and wait…");
        await new Promise(r => setTimeout(r, 2000)); // wait 2 seconds before retry
    }

    showSplash("Backend is running", "Verifying authorization…");

    const ready = await checkReady();

    hideSplash();

    ready
        ? setAuthorized()
        : setUnauthorized("API key required");
}

init();
