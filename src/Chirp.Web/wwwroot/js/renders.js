

function ToggleEditCheep(cheepId){
    let messageElem = document.getElementById("cheepmessage-" + cheepId);
    let editElem = document.getElementById("cheepedit-" + cheepId);

    if (messageElem.style.display == "none"){
        messageElem.style.display = "";
        editElem.style.display = "none";

    }else{
        messageElem.style.display = "none";
        editElem.style.display = "";
        editElem.value = messageElem.value;
        document.getElementById("cheepeditbox-" + cheepId).focus();
    }
}

