export async function getAudioDevices() {
    try {
        let devices = await navigator.mediaDevices.enumerateDevices();

        if (devices.every(d => d.kind === 'audioinput' && !d.label)) {
            await navigator.mediaDevices.getUserMedia({ audio: true });
            devices = await navigator.mediaDevices.enumerateDevices();
        }

        return devices
            .filter(device => device.kind === 'audioinput')
            .map(device => ({
                deviceId: device.deviceId,
                label: device.label || `Microphone (${device.deviceId.slice(0, 4)})`
            }));
    } catch (err) {
        console.error("Error getting audio devices:", err);
        return [];
    }
}

export async function setMicrophone(deviceId) {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({
            audio: { deviceId: { exact: deviceId } }
        });

        window._microphoneStream = stream;
    } catch (err) {
        console.error("Error setting microphone:", err);
    }
}

export function toggleMicrophone() {
    if (window.activeRoom && window.activeRoom.localParticipant) {
        window.activeRoom.localParticipant.audioTracks.forEach(publication => {
            const track = publication.track;
            if (!track) return;

            let audioPath = null;

            if (track.kind === 'audio') {
                if (track.isEnabled) {
                    track.disable();
                    audioPath = 'sounds/mute.mp3';
                } else {
                    track.enable();
                    audioPath = 'sounds/unmute.mp3';
                }
            var audio = new Audio(audioPath);
                    audio.play();
                }
            }
        });
    }
}
