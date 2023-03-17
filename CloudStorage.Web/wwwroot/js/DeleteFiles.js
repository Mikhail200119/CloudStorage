function onSelectingFile(element) {
    const deleteFilesButton = document.querySelector("button[id=delete-files-button]");

    if (element.classList.contains("selected")) {
        element.classList.remove("selected");

        if (getSelectedFileItems().length === 0) {
            deleteFilesButton.style.display = "none";
        }
    } else {
        element.classList.add("selected");
        deleteFilesButton.style.display = "inline-block";
    }
}

async function onDeletingFile() {
    const fileIdsToDelete = getSelectedFileItems()
        .map(item => item.querySelector("div[id=fileId]").textContent);

    const deleteUrl = document.querySelector("div[id=delete-file-url]").textContent;
    const queryString = buildDeleteQueryString(fileIdsToDelete);

    await fetch(`${deleteUrl}?${queryString}`, {method: 'DELETE'})
        .then(response => {
            if (!response.ok) {
                console.log("An error occured during deleting files.");
            } else {
                fileIdsToDelete.forEach(id => removeFileItemOnPage(id));

                getSelectedFileItems().forEach(item => item.classList.remove("selected"));
            }

            document.querySelector("button[id=delete-files-button]").style.display = "none";
            
            if(Array.from(document.querySelectorAll("div[id=fileDescription]")).length === 0){
                document.getElementById("no-uploaded-files-info").style.display = "block";
            }
        })
}

function removeFileItemOnPage(fileId) {
    getFileByIdFromPage(fileId).parentElement.remove();
}

function getSelectedFileItems() {
    return Array
        .from(document
            .querySelectorAll("div[id=fileDescription]"))
        .filter(item => item.classList.contains("selected"));
}


function buildDeleteQueryString(ids) {
    let query = [];

    for (let i = 0; i < ids.length; i++) {
        query.push(`ids=${ids[i]}`);

        if (i + 1 !== ids.length) {
            query.push("&");
        }
    }

    return query.join("");
}