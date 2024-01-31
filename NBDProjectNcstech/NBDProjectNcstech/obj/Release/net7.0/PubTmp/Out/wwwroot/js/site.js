// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script>
        // JavaScript to toggle the sidebar
    function toggleSidebar() {
            var sidebar = document.getElementById("sidebar");
    if (sidebar.style.left === "0px") {
        sidebar.style.left = "-250px";
            } else {
        sidebar.style.left = "0px";
            }
        }
</script>