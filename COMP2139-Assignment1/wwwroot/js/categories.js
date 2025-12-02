// categories.js

document.addEventListener("DOMContentLoaded", function () {

    const searchBox = document.getElementById("categorySearchBox");
    const resultsDiv = document.getElementById("categoryResults");

    function performSearch() {
        const search = searchBox.value;

        const url = `/Category/Search?searchString=${encodeURIComponent(search)}`;

        fetch(url)
            .then(response => response.text())
            .then(html => {
                resultsDiv.innerHTML = html;
            })
            .catch(error => console.error("Error loading categories:", error));
    }

    // Live search
    searchBox.addEventListener("keyup", performSearch);
});
