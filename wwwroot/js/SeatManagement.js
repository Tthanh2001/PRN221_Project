$(document).ready(function () {
    const seatGrid = $(".seat-grid");
    const seatTypeSelect = $("#seatType");

    seatGrid.on("click", ".seat", function () {
        const selectedSeatType = seatTypeSelect.find("option:selected");
        const seat = $(this);

        if (seat.css("background-color") === "rgba(0, 0, 0, 0)") {
            seat.css("background-color", selectedSeatType.attr("data-color"));
            seat.attr("data-seatId", selectedSeatType.val());
        } else {
            seat.css("background-color", "rgba(0, 0, 0, 0)");
            seat.attr("data-seatId", "");
        }
    });

    $("#saveButton").click(async function () {
        const selectedSeats = [];
        seatElements.each(function () {
            const seat = $(this);
            if (seat.css("background-color")) {
                selectedSeats.push({
                    row: seat.attr("data-row"),
                    col: seat.attr("data-col"),
                    typeId: seat.attr("data-seatId")
                });
            }
        });

        if (selectedSeats.length > 0) {
            try {
                await $.ajax({
                    url: "/SeatManagement",
                    method: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(selectedSeats)
                });

                // Handle success, e.g., show a success message
                console.log("Selected seats saved successfully.");
            } catch (error) {
                // Handle error, e.g., show an error message
                console.error("Error saving selected seats:", error);
            }
        }
    });
});