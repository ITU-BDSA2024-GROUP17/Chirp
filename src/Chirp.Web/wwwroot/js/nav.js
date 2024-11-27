function skipPages(pagesToSkip) {
    if (pagesToSkip == 0) throw new Error("pagesToSkip cannot be 0");
    const urlParams = new URLSearchParams(window.location.search);
    const currentPageNum = parseInt(urlParams.get("page")) || 1;
    const newPageNum = currentPageNum + pagesToSkip;
    if (newPageNum < 1) return;

    // if root page or individual author page
    if (!urlParams.has("SearchQuery")) {
        // string manipulation to remove leading slash for authors page
        window.location.href = `${window.location.pathname.replace(
            "/",
            ""
        )}?page=${newPageNum}`;
        return;
    }
    // For Search page
    const searchQuery = urlParams.get("SearchQuery") || "";
    window.location.href = `/search?SearchQuery=${searchQuery}&page=${newPageNum}`;
}

function showCommentField(id) {
    if (!Number.isInteger(id)) {
        console.error("id must be an integer.");
        return;
    }
    const fields = document.querySelectorAll(`[fieldId="${id}"]`);
    if (fields.length === 0) {
        console.error(`No elements found with fieldId: ${id}`);
        return;
    }
    fields.forEach((field) => {
        if (field.style.display === "none") {
            field.style.display = "flex";
        } else {
            field.style.display = "none";
        }
    });
}

function auto_grow(element) {
    element.style.height = "5px";
    element.style.height = element.scrollHeight - 5 + "px";
}

window.addEventListener("DOMContentLoaded", () => {
    const textLengthLimit = 160;

    document.querySelectorAll(".cheepComment").forEach((cheepInputField) => {
        const cheepInputFieldLimit = cheepInputField.nextElementSibling;
        cheepInputFieldLimit.innerHTML = "0 / " + textLengthLimit; // init length

        ["keyup", "change"].forEach((type) => {
            cheepInputField.addEventListener(type, (event) => {
                const inputFieldValue = cheepInputField.value;
                cheepInputFieldLimit.innerHTML =
                    inputFieldValue.length + " / " + textLengthLimit;
                if (inputFieldValue.length > textLengthLimit) {
                    cheepInputFieldLimit.style.color = "red";
                    cheepInputField.style.color = "red";
                } else {
                    cheepInputField.style.color = "black";
                    cheepInputFieldLimit.style.color = "var(--gray-600)";
                }
            });
        });

        cheepInputField.addEventListener("keypress", (event) => {
            if (!event.shiftKey && event.key === "Enter") {
                event.preventDefault();
                cheepInputField.closest("form").submit();
            }
        });

        auto_grow(cheepInputField);
    });
});
