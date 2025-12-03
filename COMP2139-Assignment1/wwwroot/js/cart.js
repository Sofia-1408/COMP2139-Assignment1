document.addEventListener("DOMContentLoaded", () => {

    const qty = document.getElementById("qty");
    const price = document.getElementById("price").dataset.price;
    const total = document.getElementById("total");
    const addBtn = document.getElementById("addBtn");
    const cartBadge = document.getElementById("cartBadge");
    const stockWarning = document.getElementById("lowStockWarning");

    function updateTotal() {
        const q = parseInt(qty.value);
        const p = parseFloat(price);
        total.textContent = "$" + (q * p).toFixed(2);

        const available = parseInt(stockWarning.dataset.available);

        if (available - q <= 7) {
            stockWarning.textContent = `Only ${available - q} tickets left!`;
            stockWarning.classList.remove("d-none");
        }
    }

    qty.addEventListener("input", updateTotal);

    addBtn.addEventListener("click", () => {
        const eventId = addBtn.dataset.eventid;

        fetch("/Purchase/AddToCart", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                eventId: eventId,
                quantity: qty.value
            })
        })
            .then(res => res.json())
            .then(data => {
                cartBadge.textContent = data.totalItems;
                cartBadge.classList.remove("d-none");
            });
    });

    updateTotal();
});
