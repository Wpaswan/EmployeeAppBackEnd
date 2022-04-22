using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PaymentDetailController : ControllerBase
    {
        private readonly PaymentDetailContext _context;
        public PaymentDetailController(PaymentDetailContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
            return await _context.PaymentDetails.ToListAsync();

        }

        [HttpPost("AddPaymentDetails")]
        public async Task<ActionResult<PaymentDetail>> AddPaymentDetails(PaymentDetail payment)
        {
            _context.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPaymentDetail", new { id = payment.PaymentDetailId }, payment);

            //return this.Ok(new { success = true, message = $"payment details added successfully of {payment.CardOwnerName}" });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetail==null)
            {
                return NotFound();
            }
            return paymentDetail;

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail payment)
        {
            if (id != payment.PaymentDetailId)
            {
                return BadRequest();
            }
            _context.Entry(payment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return this.Ok(new { success = true, message = $"payment details updated successfully of {payment.CardOwnerName}" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PaymentDetailExists(id))
                    return NotFound();
                else throw;

            }
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            var paymentDetail= await _context.PaymentDetails.FindAsync(id);
            if(paymentDetail==null)
            {
                return NotFound();
            }
            _context.PaymentDetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();
            return this.Ok(new { success = true, message = $"Payment Detail deleted Successfully " });
            return NoContent();
        }
        private bool PaymentDetailExists(int id)
        {
            return _context.PaymentDetails.Any(e => e.PaymentDetailId == id);
        }
    }
}
