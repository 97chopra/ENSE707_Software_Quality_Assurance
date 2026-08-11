using ENSE707_AppointmentBooking;

namespace AppointmentBooking.Persistence;

public class PersistentAppointmentBookingService
{
    private readonly AppointmentBookingService _bookingService;
    private readonly JsonBookingRepository _repository;

    public PersistentAppointmentBookingService(
        AppointmentBookingService bookingService,
        JsonBookingRepository repository)
    {
        _bookingService = bookingService;
        _repository = repository;
    }

    public BookingResult BookAppointment(AppointmentRequest request)
    {
        BookingResult result = _bookingService.BookAppointment(request);

        if (result.Success)
        {
            _repository.Add(new BookingRecord(
                request.Patient.Id,
                request.Doctor.Id,
                request.RequestedDate,
                result.Message));
        }

        return result;
    }
}