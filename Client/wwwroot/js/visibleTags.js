window.visibleTagIndexes = function () {
    const container = document.getElementById("tagContainer");
    const children = container.children;
    const visible = [];

    for (let i = 0; i < children.length; i++) {
        const child = children[i];
        const rect = child.getBoundingClientRect();
        const parentRect = container.getBoundingClientRect();

        if (rect.right <= parentRect.right) {
            visible.push(i);
        } else {
            break;
        }
    }

    console.log('visible tags:', visible);
    return visible;
};
