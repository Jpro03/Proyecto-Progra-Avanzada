(() => {
  'use strict';
  const config = window.fidepolisSeats;
  if (!config) return;
  const selected = new Set(config.selected);
  const buttons = [...document.querySelectorAll('[data-seat]')];
  const label = document.getElementById('selectedLabel');
  const quantity = document.getElementById('quantity');
  const total = document.getElementById('total');
  const input = document.getElementById('seatCodes');
  const continueButton = document.getElementById('continueButton');
  const message = document.getElementById('seatMessage');
  const currency = new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC', maximumFractionDigits: 0 });
  const render = () => {
    const seats = [...selected].sort();
    buttons.forEach(b => { const chosen = selected.has(b.dataset.seat); b.classList.toggle('selected', chosen); b.setAttribute('aria-pressed', String(chosen)); });
    label.textContent = seats.length ? seats.join(', ') : 'Ninguno';
    quantity.textContent = String(seats.length);
    total.textContent = currency.format(seats.length * config.price);
    input.value = seats.join(',');
    continueButton.disabled = seats.length === 0 || seats.length > 6;
  };
  buttons.forEach(button => button.addEventListener('click', () => {
    const seat = button.dataset.seat;
    if (button.disabled) return;
    if (selected.has(seat)) selected.delete(seat);
    else if (selected.size < 6) selected.add(seat);
    else { message.textContent = 'Podés elegir como máximo 6 asientos.'; return; }
    message.textContent = '';
    render();
  }));
  async function refresh() {
    try {
      const response = await fetch(`/Seats/Availability/${config.showtimeId}`, { cache: 'no-store' });
      if (!response.ok) return;
      const data = await response.json();
      const occupied = new Set(data.occupied);
      const lost = [...selected].filter(seat => occupied.has(seat));
      lost.forEach(seat => selected.delete(seat));
      buttons.forEach(button => { const busy = occupied.has(button.dataset.seat) || !data.open; button.disabled = busy; button.classList.toggle('occupied', busy); });
      if (lost.length) message.textContent = `Los asientos ${lost.join(', ')} ya están ocupados y se quitaron de tu selección.`;
      else if (!data.open) message.textContent = 'Esta función ya comenzó.';
      render();
      if (!data.open) continueButton.disabled = true;
    } catch { message.textContent = 'No se pudo actualizar la disponibilidad. Se verificará al confirmar.'; }
  }
  document.getElementById('seatForm').addEventListener('submit', e => { if (!selected.size) e.preventDefault(); });
  render(); refresh(); setInterval(refresh, 8000);
})();
