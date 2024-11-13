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
