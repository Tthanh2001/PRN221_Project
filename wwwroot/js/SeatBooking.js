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
            seat.attr("data-is-booked", "true");
            
            numberSeats++;
        } else {
            seat.css("background-color", "rgba(0, 0, 0, 0)");
            seat.attr("data-is-booked", "false");
            numberSeats--;
        }
    });

   
});
