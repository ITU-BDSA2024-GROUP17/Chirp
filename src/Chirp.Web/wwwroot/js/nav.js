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

window.addEventListener("DOMContentLoaded", () => {
    const fields = document.querySelectorAll("#cheepComment");
    fields.forEach((comment) => {
        const textLengthLimit = 160;
        const cheepInputFieldLimit =
            comment.parentElement.querySelector(".text-box-limit");

        cheepInputFieldLimit.innerHTML = "0 / " + textLengthLimit; // init length

        ["keypress", "change"].forEach((type) => {
            comment.addEventListener(type, (event) => {
                const submitBtn =
                    comment.parentElement.parentElement.querySelector(
                        "#sendCommentButton"
                    );
                if (comment.value.length > textLengthLimit) {
                    submitBtn.disabled = true;
                } else {
                    submitBtn.disabled = false;
                }

                if (!event.shiftKey && event.key == "Enter") {
                    event.preventDefault();
                    if (comment.value.length > textLengthLimit) {
                        return;
                    }
                    comment.parentElement.parentElement.submit();
                }
                auto_grow(comment);
                auto_grow(comment.parentElement.parentElement);

                auto_grow(comment.parentElement.parentElement.parentElement);
                const inputFieldValue = comment.value;
                cheepInputFieldLimit.innerHTML =
                    inputFieldValue.length + " / " + textLengthLimit;
                if (inputFieldValue.length > textLengthLimit) {
                    cheepInputFieldLimit.style.color = "red";
                    comment.style.color = "red";
                } else {
                    comment.style.color = "black";
                    cheepInputFieldLimit.style.color = "var(--gray-600)";
                }
            });
        });
    });
});
