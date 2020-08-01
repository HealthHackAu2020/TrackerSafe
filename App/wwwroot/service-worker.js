// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', () => { });

self.addEventListener('push', event => {
  const payload = event.data.json();
  console.log('Got Push: ' + JSON.stringify(event.data));
  event.waitUntil(
    self.registration.showNotification('Tracker Safe (Dev)', {
      body: payload.message,
      icon: 'img/icon-512.png',
      vibrate: [100, 50, 100],
      data: { url: payload.url }
    })
  );
});

self.addEventListener('notificationclick', event => {
  console.log('Clicked Push: ' + JSON.stringify(event));
  event.notification.close();
  event.waitUntil(clients.openWindow("https://healthhackau2020.github.io/TrackerSafe/view-notifications"));
  console.log('Clicked Push Done!');
});