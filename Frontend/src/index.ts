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
    const year = yearInput.value;
    const selectedCategories: string[] = [];
    
    if (!isYearValid(year)) {
        alert("Год должен состоять из 4 цифр");
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
    return true; // optional field
  }

  return /^\d{4}$/.test(year);
}
