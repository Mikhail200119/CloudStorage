// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function setupContextMenu() {
    const contextMenu = document.getElementById("context-menu");
    const fileItems = document.querySelectorAll("div[class=fileItem]");
    Array.from(fileItems)
        .forEach(fileItem =>
        {
            fileItem.addEventListener("contextmenu", event =>
            {
                event.preventDefault();

                const ids = contextMenu.querySelectorAll("div[id=fileId]");
                if (ids !== null) {
                    Array.from(ids)
                        .forEach(id => id.remove());
                }

                const fileIdDiv = document.createElement("div");
                fileIdDiv.id = "fileId";
                fileIdDiv.style.display = "none";
                fileIdDiv.textContent = fileItem.querySelector("div[id=fileId]").textContent;
                contextMenu.appendChild(fileIdDiv);
                const { clientX: mouseX, clientY: mouseY } = event;
                contextMenu.style.top = `${mouseY}px`;
                contextMenu.style.left = `${mouseX}px`;

                contextMenu.classList.add("visible");

                setTimeout(() => contextMenu.classList.add("visible"));
            });
        });

    const html = document.querySelector("html");
    html.addEventListener("click", () =>
    {
        const menu = document.getElementById("context-menu");
        menu.classList.remove("visible");
        Array.from(menu.querySelectorAll("div[id=fileId]"))
            .forEach(idDiv => idDiv.remove());
    });
}