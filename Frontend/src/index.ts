const titleInput = document.getElementById("movie-title-en") as HTMLInputElement;
const desctiptionInput = document.getElementById("movie-description") as HTMLInputElement;
const yearInput = document.getElementById("release-year") as HTMLInputElement;

const checkboxes = document.querySelectorAll(
    ".checkbox-container input[type='checkbox']"
) as NodeListOf<HTMLInputElement>;

const clearButton = document.getElementById("clear") as HTMLButtonElement;
const addButton = document.getElementById("add") as HTMLButtonElement;

clearButton.addEventListener("click", clearInput);
addButton.addEventListener("click", sendToAnytype)

function sendToAnytype() {
    const selectedCategories: string[] = [];

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
    
}

function clearInput() {
    titleInput.value = "";
    desctiptionInput.value = "";
    yearInput.value = "";

    checkboxes.forEach(cb => {
        cb.checked = false;
    });
}
