$(document).ready(function () {
    setTimeout(hideMyDiv, 2500); // Hide the div after N seconds
});

// Function to fade out the div
function hideMyDiv() {
    $('#success__msg').fadeOut('slow');
    $('#error__msg').fadeOut('slow');
}
