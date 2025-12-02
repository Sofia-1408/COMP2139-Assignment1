document.addEventListener("DOMContentLoaded", function () {

    const searchBox = document.getElementById("searchBox");
    const categorySelect = document.getElementById("categoryId");
    const resultsDiv = document.getElementById("eventResults");

    function performSearch() {
        const search = searchBox.value;
        const categoryId = categorySelect.value;

        const url = `/Event/Search?userSearch=${encodeURIComponent(search)}&categoryId=${categoryId}`;

        fetch(url)
            .then(response => response.text())
            .then(html => {
                resultsDiv.innerHTML = html;
            })
            .catch(error => console.error("Error loading events:", error));
    }

    // Live search typing
    searchBox.addEventListener("keyup", performSearch);

    // Update when category changes
    categorySelect.addEventListener("change", performSearch);
});