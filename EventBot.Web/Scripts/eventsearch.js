var costButtonMode = "free";
window.addEventListener("load", () => {
    $('#eventSearchButton').click(() => {
        eventSearch();
    });
    $('.search-input').keyup(() => {
        UpdateCostButton();
        eventSearch();
    });
    $('#MaxCost').change(() => {
        UpdateCostButton();
        eventSearch();
    });
    $('#searchPersons').change(() => {
        eventSearch();
    });
    eventSearch();
});

function UpdateCostButton() {
    var maxCost = $('#MaxCost').val();
    if (maxCost == null||maxCost=='') {
        $('#freeButton').val('Visa bara gratis');
        costButtonMode = "free";
    } else {
        $('#freeButton').val('Visa alla');
        costButtonMode = "all";
    }
}

function ToggleAdvancedSearch() {

    var advancedDiv = document.getElementById('advancedSearchContainer');
    if (advancedDiv.style.display != 'none')
        advancedDiv.style.display = 'none';
    else
        advancedDiv.style.display = 'inherit';
    return false;
}
function CostButtonAction() {
    if (costButtonMode == "free") {
        $('#MaxCost').val(0);
    } else {
        $('#MaxCost').val('');
    }
    UpdateCostButton();
    eventSearch();
}

function eventSearch() {
    var query = $('#query').val();
    var maxCost = $('#MaxCost').val();
    var minPlaces = $('#searchPersons').val();
    $('#EventTable').load('/Event/Search/', {
        query: query,
        persons: minPlaces,
        cost: maxCost
    });
}