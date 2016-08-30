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
    var onlyFree = $('#searchOnlyFreeEvents').prop('checked');
    var minPlaces = $('#searchPersons').val();
    $('#EventTable').load('/Event/Search/', {
        query: query,
        persons: minPlaces,
        cost: onlyFree ? 0 : 1000000
    });
}