

function ToggleEditCheep(cheepId){
    let messageElem = document.getElementById("cheepmessage-" + cheepId);
    let editElem = document.getElementById("cheepedit-" + cheepId);

    let revisionList = document.getElementById("cheeprevision-" + cheepId);
    revisionList.style.display = "none";

    //If the edit input box isn't shown. Enable it and disable the original cheepMessage box
    if (messageElem.style.display == ""){
        messageElem.style.display = "none";
        editElem.style.display = "";
        document.getElementById("cheepeditbox-" + cheepId).focus();
    }else{
        messageElem.style.display = "";
        editElem.style.display = "none";
    }
}

function ToggleRevisions(cheepId){
    if (document.getElementById("cheepedit-" + cheepId).style.display == "") { return; }

    console.log("Toggle Edit!");
    let revisionList = document.getElementById("cheeprevision-" + cheepId);
    let messageElem = document.getElementById("cheepmessage-" + cheepId);

    if (revisionList.style.display == "none"){
        messageElem.style.display = "none";
        revisionList.style.display = "";
    }else{
        revisionList.style.display = "none";
        messageElem.style.display = "";
    }
}

