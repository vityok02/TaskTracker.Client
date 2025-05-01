export function isMemberDefined(instance, member) {
    return !!instance && instance[member] !== undefined;
}