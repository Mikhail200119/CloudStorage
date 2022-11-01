function onSearchInput(element) {
    document
        .querySelectorAll("div[id=fileDescription]")
        .forEach(fileDescription => {
            const fileNameDiv = fileDescription.querySelector("div[id=fileName]");

            if (fileNameDiv.innerText.includes(element.value)) {
                if (fileDescription.getAttribute("hidden")) {
                    fileDescription.removeAttribute("hidden");
                }
            } else {
                fileDescription.setAttribute("hidden", "hidden");
            }
        });
}