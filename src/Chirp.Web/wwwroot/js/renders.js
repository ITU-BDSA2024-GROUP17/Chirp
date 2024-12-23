/**
 * Toggles edit mode for a cheep
 * @param {int} cheepId The id of the cheep to toggle edit for
 */
function ToggleEditCheep(cheepId) {
    let messageElem = document.getElementById("cheepmessage-" + cheepId);
    let editElem = document.getElementById("cheepedit-" + cheepId);

    let revisionList = document.getElementById("cheeprevision-" + cheepId);
    revisionList.style.display = "none";

    //If the edit input box isn't shown. Enable it and disable the original cheepMessage box
    if (messageElem.style.display == "") {
        messageElem.style.display = "none";
        editElem.style.display = "";
        document.getElementById("cheepeditbox-" + cheepId).focus();
    } else {
        messageElem.style.display = "";
        editElem.style.display = "none";
    }
}

/**
 * Toggles revision list for a cheep
 * @param {int} cheepId The id of the cheep to toggle revisions for
 * @returns
 */
function ToggleRevisions(cheepId) {
    if (document.getElementById("cheepedit-" + cheepId).style.display == "") {
        return;
    }

    let revisionList = document.getElementById("cheeprevision-" + cheepId);
    let messageElem = document.getElementById("cheepmessage-" + cheepId);

    if (revisionList.style.display == "none") {
        messageElem.style.display = "none";
        revisionList.style.display = "";
    } else {
        revisionList.style.display = "none";
        messageElem.style.display = "";
    }
}

$(document).ready(function () {
    $(".dropdown").hover(
        function () {
            $(this).addClass("show");
            $(this).find(".dropdown-menu").addClass("show");
        },
        function () {
            $(this).removeClass("show");
            $(this).find(".dropdown-menu").removeClass("show");
        }
    );
});

function startHoverLogo() {
    setTimeout(() => {
        document.querySelector(".logo").src = "/images/icon1EasetEgg.png";
    }, 1250);
}

function endHoverLogo() {
    setTimeout(() => {
        document.querySelector(".logo").src = "/images/icon1.png";
    }, 500);
}
