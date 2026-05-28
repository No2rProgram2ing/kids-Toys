// Start Header Section

const lis = document.querySelectorAll(".navbar .container .navbar-nav li .nav-link");

const activeLink = localStorage.getItem("activeNavLink");
if (activeLink) {
    lis.forEach((li) => {
        if (li.getAttribute("data-active") === activeLink) {
            li.classList.add("active");
        } else {
            li.classList.remove("active");
        }
    });
}

lis.forEach((li) => {
    li.addEventListener("click", () => {
        lis.forEach((li) => li.classList.remove("active"));

        li.classList.add("active");

        const dataActive = li.getAttribute("data-active");
        localStorage.setItem("activeNavLink", dataActive);
    });
});


// End Header Section
