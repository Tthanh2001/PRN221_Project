$(document).ready(function () {
    const seatGrid = $("#seat-grid");
    const seatTypeSelect = $("#seatType");

    seatGrid.on("click", ".seat", function () {
        const selectedSeatType = seatTypeSelect.find("option:selected");
        const seat = $(this);

        if (seat.css("background-color") === "rgba(0, 0, 0, 0)") {
            seat.css("background-color", selectedSeatType.attr("data-color"));
            seat.attr("data-seatTypeId", selectedSeatType.val());
        } else {
            seat.css("background-color", "rgba(0, 0, 0, 0)");
            seat.attr("data-seatTypeId", "");
        }
    });

    $("#saveButton").click(function () {
        const selectedSeats = [];
        seatGrid.find(".seat").each(function () {
            const seat = $(this);
            const seatId = seat.attr("data-seatTypeId");

            if (seatId) {
                const seatData = {
                    row: seat.attr("data-row"),
                    col: seat.attr("data-col"),
                    typeId: seatId,
                    available: seat.attr("data-availabe")
                };
                selectedSeats.push(seatData);
            }
        });

        if (selectedSeats.length > 0) {
            try {
                $.ajax({
                    url: "/SeatManagement",
                    method: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(selectedSeats),
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    success: function (response) {
                        console.log(response);
                    },
                    error: function (error) {
                        console.log("Error:", error);
                    }
                });
                console.log("Selected seats saved successfully.");
            } catch (error) {
                // Handle error, e.g., show an error message
                console.log("Error saving selected seats:", error);
            }
        }
    });
});