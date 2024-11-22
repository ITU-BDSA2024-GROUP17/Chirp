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
