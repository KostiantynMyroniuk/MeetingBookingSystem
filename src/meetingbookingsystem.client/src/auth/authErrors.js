export function extractErrorMessage(error, fallback) {
    const data = error?.response?.data;

    if (data?.errors) {
        const messages = Object.values(data.errors).flat();
        if (messages.length > 0) {
            return messages.join(' ');
        }
    }

    if (error?.response?.status === 401) {
        return 'Невірний email або пароль';
    }

    if (typeof data?.title === 'string' && data.title.length > 0) {
        return data.title;
    }

    return fallback;
}
