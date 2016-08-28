window.addEventListener("load", () => {
    $('#eventSearchButton').click(() => {
        eventSearch();
    });
    $('#query').keyup(() => {
        eventSearch();
    });
    eventSearch();
});

function eventSearch() {
    var query = $('#query').val();
    $('#EventTable').load('/Event/Search/', { query:query });
}