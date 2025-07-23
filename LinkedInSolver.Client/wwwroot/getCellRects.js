window.getCellRects = (elements) => {
    return Array.from(elements).map(el => {
        if (!el || typeof el.getBoundingClientRect !== 'function') {
            return { top: 0, left: 0, width: 0, height: 0 };
        }
        const rect = el.getBoundingClientRect();
        return {
            top: rect.top + window.scrollY,
            left: rect.left + window.scrollX,
            width: rect.width,
            height: rect.height
        };
    });
};
