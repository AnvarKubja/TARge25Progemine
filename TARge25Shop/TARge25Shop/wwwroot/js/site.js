// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Märgib menüüs aktiivse lehe
const currentSection = location.pathname.split('/')[1].toLowerCase() || 'home';

document.querySelectorAll('.navbar a.nav-link').forEach(link => {
    const parts = new URL(link.href).pathname.split('/');
    const section = parts[1].toLowerCase() || 'home';

    // Home, Account ja Privacy puhul peab ka alamleht klappima
    const samePage = link.pathname.toLowerCase() === location.pathname.toLowerCase();
    const isShared = section === 'home' || section === 'account';

    if (link.getAttribute('href') !== '#' && (isShared ? samePage : section === currentSection)) {
        link.classList.add('active');
    }
});
