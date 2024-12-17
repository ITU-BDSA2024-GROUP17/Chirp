document.addEventListener("DOMContentLoaded", function () {
    resetkeydownIndex();

    const searchInput = document.getElementById("navSearchInput");
    const navSearchOutputWrapper = document.getElementsByClassName(
        "navSearchOutputWrapper"
    );

    searchInput.addEventListener("focus", function () {
        resetkeydownIndex();

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
        resetkeydownIndex();

        setTimeout(() => {
            const navSearchOutputWrapper = document.getElementsByClassName(
                "navSearchOutputWrapper"
            );
            navSearchOutputWrapper[0].style.display = "none";
        }, 150);
    });

    searchInput.addEventListener("input", async function (event) {
        resetkeydownIndex();

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
            renderAuthorsInSearch(data);
        } catch (error) {
            console.error(
                "There was a problem with the fetch operation:",
                error
            );
        }
    });
    navSearchOutputWrapper[0].addEventListener("mouseover", function (event) {
        listItems[currentLI].classList.remove("search-highlight");
        resetkeydownIndex();
    });

    document.addEventListener("keydown", function (event) {
        listItems = document.getElementsByClassName("cheep-wrapper-mini");
        // Check for up/down key presses
        if (
            document.activeElement.id != "navSearchInput" &&
            document.activeElement.className != "cheep-wrapper-mini"
        ) {
            return;
        }
        switch (event.keyCode) {
            case 38: // Up arrow
                if (currentLI == 0 || currentLI == -1) {
                    event.preventDefault();

                    listItems[currentLI].classList.remove("search-highlight");
                    currentLI = -1;
                    break;
                }
                // Remove the highlighting from the previous element
                listItems[currentLI].classList.remove("search-highlight");

                currentLI = currentLI > 0 ? --currentLI : 0; // Decrease the counter
                listItems[currentLI].classList.add("search-highlight"); // Highlight the new element
                break;
            case 40: // Down arrow
                // Remove the highlighting from the previous element
                if (currentLI == -1) {
                    currentLI = 0;
                    listItems[currentLI].classList.add("search-highlight"); // Highlight the new element
                    break;
                }
                listItems[currentLI].classList.remove("search-highlight");

                currentLI =
                    currentLI < listItems.length - 1
                        ? ++currentLI
                        : listItems.length - 1; // Increase counter
                listItems[currentLI].classList.add("search-highlight"); // Highlight the new element
                break;
            case 13: // Enter key
                if (currentLI == -1) {
                    const searchQuery = searchInput.value;
                    window.location.href = `/search?SearchQuery=${encodeURIComponent(
                        searchQuery
                    )}`;
                } else {
                    window.location.href = `${listItems[currentLI].href}`;
                }
                break;
        }
    });

    /**
     * Resets the currentLI to -1 and clears the listItems array
     */
    function resetkeydownIndex() {
        currentLI = -1;
        listItems = [];
    }
});

/**
 * Function to render the search results that drop down from the search bar
 * @param {CreateAuthorDto[]} data
 */
function renderAuthorsInSearch(data) {
    data.forEach((user) => {
        let name = user.userName;
        let avatar = user.avatar;
        const a = document.createElement("a");
        a.className = "cheep-wrapper-mini";
        a.href = `/${name}`;
        if (avatar == null) {
            a.innerHTML = `
                            <div class="avatar">
                                ${name.charAt(0)}
                            </div>
                            <div class="cheep">
                                <div class="cheep-user">
                                    <div class="cheep-author">${name}</div>
                                </div>
                            </div>
                        `;
        } else {
            a.innerHTML = `
                        <div class="avatar">
                            <img src="${avatar}" class="avatar"/>
                        </div>
                        <div class="cheep">
                            <div class="cheep-user">
                                <div class="cheep-author">${name}</div>
                            </div>
                        </div>
                    `;
        }
        navSearchOutput.appendChild(a);
    });
}
