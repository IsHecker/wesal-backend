// ========================================
// CONFIGURATION - EDIT THESE VALUES
// ========================================
const firebaseConfig = {
    apiKey: "AIzaSyCmJqC-EhQBfpvg_nFLbfMU0oOgO-EYzTU",
    authDomain: "wesal-83d46.firebaseapp.com",
    projectId: "wesal-83d46",
    storageBucket: "wesal-83d46.firebasestorage.app",
    messagingSenderId: "188164569064",
    appId: "1:188164569064:web:00a969a0935dd57641565d"
};
// ========================================

// Import Firebase scripts for service worker
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging-compat.js');

// Initialize Firebase in service worker
firebase.initializeApp(firebaseConfig);

// Retrieve an instance of Firebase Messaging
const messaging = firebase.messaging();

// Handle background messages
messaging.onBackgroundMessage((payload) => {
    console.log('Received background message:', payload);

    // Send message to all open tabs/windows
    self.clients.matchAll({ includeUncontrolled: true, type: 'window' })
        .then(clients => {
            clients.forEach(client => {
                client.postMessage({
                    type: 'FCM_MESSAGE',
                    payload: payload
                });
            });
        });

    // Show notification
    const notificationTitle = payload.notification?.title || 'New Notification';
    const notificationOptions = {
        body: payload.notification?.body || '',
        icon: '/icon.png',
        data: payload.data
    };

    self.registration.showNotification(notificationTitle, notificationOptions);
});

// Handle notification click
self.addEventListener('notificationclick', (event) => {
    console.log('Notification clicked:', event);

    event.notification.close();

    // Open the app or focus existing window
    event.waitUntil(
        clients.matchAll({ type: 'window', includeUncontrolled: true })
            .then((clientList) => {
                // If a window is already open, focus it
                for (const client of clientList) {
                    if (client.url.includes(self.location.origin) && 'focus' in client) {
                        return client.focus();
                    }
                }
                // Otherwise open a new window
                if (clients.openWindow) {
                    return clients.openWindow('/');
                }
            })
    );
});