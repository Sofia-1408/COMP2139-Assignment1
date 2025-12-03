document.addEventListener("DOMContentLoaded", () => {

    //This is a chart that displays ticket sales based on the categories
    fetch("/Event/GetSalesByCategory")
        .then(res => res.json())
        .then(data => {
            new Chart(document.getElementById("salesByCategoryChart"), {
                type: "doughnut",
                data: {
                    labels: data.map(x => x.category),
                    datasets: [{
                        data: data.map(x => x.ticketsSold),
                        backgroundColor: ["#ff6384", "#36a2eb", "#ffce56", "#9c27b0", "#4caf50"]
                    }]
                }
            });

        });

    //This is a chart that shows revenues per month based on categories
    fetch("/Event/GetRevenuePerMonth")
        .then(res => res.json())
        .then(data => {
            console.log("Revenue data:", data);
            new Chart(document.getElementById("revenueChart"), {
                type: "line",
                data: {
                    labels: data.map(x => x.month),
                    datasets: [{
                        label: "Revenue ($)",
                        data: data.map(x => x.revenue),
                        borderColor: "#36a2eb",
                        borderWidth: 2,
                        fill: false
                    }]
                }
            });

        });

    //These display top 5 best selling events in a table
    fetch("/Event/GetTopEvents")
        .then(res => res.json())
        .then(data => {

            const tbody = document.querySelector("#topEventsTable tbody");
            tbody.innerHTML = "";

            data.forEach(ev => {
                tbody.innerHTML += `
                <tr>
                    <td>${ev.title}</td>
                    <td>${ev.sold}</td>
                </tr>`;
            });
        });

});
