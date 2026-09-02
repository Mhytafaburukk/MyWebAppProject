
function saveUserPreferences() {
    const preferences = {
        theme: document.getElementById('themeSelect').value,
        fontSize: document.getElementById('fontSizeRange').value,
        notifications: document.getElementById('notificationsCheck').checked
    };

    localStorage.setItem('userPreferences', JSON.stringify(preferences));
    showMessage('Preferences saved locally!', 'success');
}

function loadUserPreferences() {
    const savedPreferences = localStorage.getItem('userPreferences');

    if (savedPreferences) {
        const preferences = JSON.parse(savedPreferences);

        document.getElementById('themeSelect').value = preferences.theme || 'light';
        document.getElementById('fontSizeRange').value = preferences.fontSize || '16';
        document.getElementById('notificationsCheck').checked = preferences.notifications || false;

        applyTheme(preferences.theme);
    }
}