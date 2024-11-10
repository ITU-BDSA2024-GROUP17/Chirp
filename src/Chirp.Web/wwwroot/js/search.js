document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("navSearchInput");
    const navSearchOutputWrapper = document.getElementsByClassName(
        "navSearchOutputWrapper"
    );

    searchInput.addEventListener("focus", function () {
        const navSearchOutputWrapper = document.getElementsByClassName(
            "navSearchOutputWrapper"
        );

        if (searchInput.value.length > 0) {
            navSearchOutputWrapper[0].style.display = "block";
        } else {
            navSearchOutputWrapper[0].style.display = "none";
        }
    });

    searchInput.addEventListener("focusout", function () {
        setTimeout(() => {
            const navSearchOutputWrapper = document.getElementsByClassName(
                "navSearchOutputWrapper"
            );
            navSearchOutputWrapper[0].style.display = "none";
        }, 150);
    });

    searchInput.addEventListener("input", async function (event) {
        const searchQuery = event.target.value;
        try {
            const response = await fetch(
                `/searchField?searchQuery=${encodeURIComponent(
                    searchQuery
                )}&page = 1`
            );
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            navSearchOutputWrapper[0].style.display = "block";
            if (data.length === 0) {
                const navSearchOutput =
                    document.getElementById("navSearchOutput");
                navSearchOutput.innerHTML = "No results found";
                return;
            }

            const navSearchOutput = document.getElementById("navSearchOutput");
            navSearchOutput.innerHTML = "";
            renderAuthorsInSeach(data);
        } catch (error) {
            console.error(
                "There was a problem with the fetch operation:",
                error
            );
        }
    });
});

function renderAuthorsInSeach(data) {
    data.forEach((user) => {
        let name = user.userName;

        const a = document.createElement("a");
        a.className = "cheep-wrapper-mini";
        a.href = `${name}`;
        a.innerHTML = `
                        <div class="cheep-avatar">
                            ${name.charAt(0)}
                        </div>
                        <div class="cheep">
                            <div class="cheep-user">
                                <div class="cheep-author">${name}</div>
                            </div>
                        </div>
                    `;
        navSearchOutput.appendChild(a);
    });
}
