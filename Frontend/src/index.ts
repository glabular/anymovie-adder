const titleInput = document.getElementById("movie-title-en") as HTMLInputElement;
const desctiptionInput = document.getElementById("movie-description") as HTMLInputElement;
const yearInput = document.getElementById("release-year") as HTMLInputElement;

const checkboxes = document.querySelectorAll(
    ".checkbox-container input[type='checkbox']"
) as NodeListOf<HTMLInputElement>;

const clearButton = document.getElementById("clear") as HTMLButtonElement;
const addButton = document.getElementById("add") as HTMLButtonElement;
const toast = document.getElementById('toast');
const toastTitle = toast?.querySelector('.toast-title');
const toastMessage = toast?.querySelector('.toast-message');
const toastCloseButton = document.getElementById('toast-close') as HTMLButtonElement;

clearButton.addEventListener("click", clearInput);
addButton.addEventListener("click", sendToAnytype);
toastCloseButton.addEventListener("click", hideToast);

// Process Enter key on text fields.
const inputs = [titleInput, desctiptionInput, yearInput];

inputs.forEach(input => {
    input.addEventListener("keydown", (e: KeyboardEvent) => {
        if (e.key === "Enter") {
            e.preventDefault();
            sendToAnytype();
        }
    });
});

let toastTimeout: number | undefined;

function sendToAnytype() {
    const title = titleInput.value;
    const year = yearInput.value;
    const selectedCategories: string[] = [];
    
    if (!isTitleValid(title)) {
        showToast('Ошибка', 'Необходимо ввести тайтл');
        titleInput.focus();

        return;
    }

    if (!isYearValid(year)) {        
        showToast('Ошибка', "Год должен состоять из 4 цифр");
        yearInput.focus();        
        yearInput.select();

        return;        
    }

    checkboxes.forEach(cb => {
        if (cb.checked) {
            selectedCategories.push(cb.value);
        }
    });

    const movieData = {
        titleEn: titleInput.value,
        description: desctiptionInput.value,
        releaseYear: yearInput.value,
        categories: selectedCategories
    }    

    console.log(movieData);
    
    clearInput();
    
    titleInput.focus();
}

function clearInput() {
    titleInput.value = "";
    desctiptionInput.value = "";
    yearInput.value = "";

    checkboxes.forEach(cb => {
        cb.checked = false;
    });
}

function isYearValid(year: string): boolean {
  if (year.trim() === "") {
    return true; // Year is optional field
  }

  return /^\d{4}$/.test(year);
}

function isTitleValid(title:string): boolean {
    return title.trim().length > 0;
}

function showToast(title:string, message: string) {
    if (!(toast instanceof HTMLElement)) {
        throw new Error('Toast element not found');
    }
    if (toastTitle instanceof HTMLElement) {
        toastTitle.textContent = title;
    }
    if (toastMessage instanceof HTMLElement) {
        toastMessage.textContent = message;
    }

    toast.style.transform = "translateX(0px)";

    if (toastTimeout) {
        clearTimeout(toastTimeout);
    }

    toastTimeout = window.setTimeout(() => {
        hideToast();
    }, 3500);
}

function hideToast () {  
    if (!(toast instanceof HTMLElement)) {
        throw new Error('Toast element not found');
    }  
    
    toast.style.transform = "translateX(110%)";
}