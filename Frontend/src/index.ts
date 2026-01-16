const titleInput = document.getElementById("movie-title-en") as HTMLInputElement;
const desctiptionInput = document.getElementById("movie-description") as HTMLInputElement;
const yearInput = document.getElementById("release-year") as HTMLInputElement;

const checkboxes = document.querySelectorAll(
    ".checkbox-container input[type='checkbox']"
) as NodeListOf<HTMLInputElement>;

const clearButton = document.getElementById("clear") as HTMLButtonElement;

clearButton.addEventListener("click", clearInput);

function clearInput() {
    titleInput.value = "";
    desctiptionInput.value = "";
    yearInput.value = "";

    checkboxes.forEach(cb => {
        cb.checked = false;
    });
}
