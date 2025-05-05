import { isMemberDefined } from "./isMemberDefined.js";

export async function getVideoDevices() {
    try {
        let devices = await navigator.mediaDevices.enumerateDevices();
        if (devices.every(d => !d.label)) {
            await navigator.mediaDevices.getUserMedia({
                video: {
                    width: { ideal: 1280 },
                    height: { ideal: 720 },
                    aspectRatio: 16/9,
                }
            });
        }

        devices = await navigator.mediaDevices.enumerateDevices();
        if (devices && devices.length) {
            const deviceResults = [];
            devices.filter(device => device.kind === 'videoinput')
                .forEach(device => {
                    const { deviceId, label } = device;
                    deviceResults.push({ deviceId, label });
                });

            return deviceResults;
        }
    } catch (error) {
        console.log(error);
    }

    return [];
}

export async function startVideo(deviceId, selector) {
    const cameraContainer = document.querySelector(selector);
    if (!cameraContainer) {
        return;
    }

    try {
        if (window.videoTrack) {
            window.videoTrack.stop();
            window.videoTrack.detach().forEach(element => element.remove());
        }

        window.videoTrack = await Twilio.Video.createLocalVideoTrack({ deviceId });

        const existingVidelEl = document.querySelector('#camera video');
        const videoEl = window.videoTrack.attach(existingVidelEl);
        styleVideo(videoEl);

        cameraContainer.append(videoEl);

        if (window.activeRoom && window.activeRoom.localParticipant) {
            await window.activeRoom.localParticipant.publishTrack(window.videoTrack);
        }
    } catch (error) {
        console.log(error);
    }
}

export async function toggleCamera(deviceId) {
    const videoContainerSelector = '#camera';

    if (window.videoTrack && window.videoTrack.mediaStreamTrack.readyState === 'live') {
        const existingVideoEl = document.querySelector('#camera video');
        if (existingVideoEl) {
            existingVideoEl.remove();
        }

        window.videoTrack.stop();
        window.videoTrack = null;

        if (window.activeRoom && window.activeRoom.localParticipant) {
            window.activeRoom.localParticipant.videoTracks.forEach(publication => {
                publication.track.stop();
                publication.unpublish();

            });

            window.activeRoom.participants.forEach(participant => {
                participant.videoTracks.forEach(publication => {
                    const videoEl = publication.track.attach();
                    videoEl.remove();
                });
            });

        }
        console.log('Camera is now off');

        return;
    }

    if (deviceId) {
        await startVideo(deviceId, videoContainerSelector);
        console.log('Camera is now on');

        if (window.activeRoom && window.activeRoom.localParticipant) {
            await window.activeRoom.localParticipant.publishTrack(window.videoTrack);

            return
        }
        console.warn('No camera device ID available');
    }
}

export function attachTrack(track, participantSid) {
    if (isMemberDefined(track, 'attach')) {
        const audioOrVideo = track.attach();
        audioOrVideo.id = track.sid;

        const containerId = `participant-${participantSid}`;

        let container = document.getElementById(containerId);
        if (!container) {
            container = document.createElement('div');
            container.classList.add('ratio', 'ratio-16x9');
            container.id = containerId;
            document.getElementById('participants').appendChild(container);
        }

        if (audioOrVideo.tagName.toLowerCase() === 'video') {
            styleVideo(audioOrVideo);
        }

        container.appendChild(audioOrVideo);
    }
}

export function detachTrack(track, participantSid) {
    if (isMemberDefined(track, 'detach')) {
        track.detach().forEach(el => el.remove());

        const containerId = `participant-${participantSid}`;
        const container = document.getElementById(containerId);
        if (container) {
            container.remove();
        }
    }
}

function styleVideo(el) {
    el.style.width = '100%';
    el.style.height = '100%';
    el.style.objectFit = 'cover';
    el.style.borderRadius = '10px';

    const aspectRatio = 16 / 9;
    el.style.aspectRatio = aspectRatio.toString();
}