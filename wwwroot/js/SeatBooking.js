$(document).ready(function () {
    const seatGrid = $("#seat-grid");
    var numberSeats = 1;

    seatGrid.on("click", ".seat-available", function () {
        const seat = $(this);

        if (seat.css("background-color") === "rgba(0, 0, 0, 0)") {
            if (numberSeats > 10) {
                alert("You only can choose max 10 seats");
                return;
            }

            seat.css("background-color", seat.attr("data-color"));
            seat.attr("data-is-booked", true);

            numberSeats++;
        } else {

            seat.css("background-color", "rgba(0, 0, 0, 0)");
            seat.attr("data-is-booked", false);
            numberSeats--;

        }
    });

    $("#saveButton").click(function () {
        const selectedSeats = [];
        seatGrid.find(".seat-available").each(function () {
            const seat = $(this);
            var seatId = seat.data("seatId");

            if (seat.data("isBooked")) {
                selectedSeats.push(seatId);
            }
        });

        //Post to backend
        if (selectedSeats.length > 0) {
            var data = JSON.stringify(selectedSeats)
            window.location.href = `/Booking/Payment?selectedSeats=${data}&scheduleId=${movieScheduleId}`;
        }
    });


});
