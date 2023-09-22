import { ref } from "vue";

export type CountdownOptions = {
  interval?: number;
  id?: string;
};
export type CountdownStatus =
  | "initial"
  | "progressing"
  | "paused"
  | "finished"
  | "stopped";

export function useCountdown(duration = 60, options: CountdownOptions = {}) {
  options.interval = options.interval || 200;
  const countdown = ref(duration);
  const status = ref<CountdownStatus>("initial");

  let countdownId = "";
  if (options.id) {
    countdownId = `countdown-${options.id}`;
  }

  let startTime = 0,
    timer = 0;
  const start = () => {
    if (status.value === "progressing" && timer) {
      return;
    }
    if (
      status.value === "initial" ||
      status.value === "finished" ||
      status.value === "stopped"
    ) {
      startTime = Date.now();
      countdown.value = duration;
      if (countdownId) {
        uni.setStorageSync(countdownId, startTime);
      }
    }

    status.value = "progressing";
    timer = setInterval(() => {
      const now = Date.now();
      const elapsed = now - startTime;
      if (elapsed / 1000 > duration) {
        status.value = "finished";
        clearInterval(timer);
        countdown.value = 0;
      }
      countdown.value = Math.ceil(duration - elapsed / 1000);
    }, options.interval);
  };
  const pause = () => {
    clearInterval(timer);
    status.value = "paused";
  };
  const stop = () => {
    clearInterval(timer);
    countdown.value = 0;
    status.value = "stopped";
    if (countdownId) {
      uni.removeStorageSync(countdownId);
    }
  };
  const reset = () => {
    clearInterval(timer);
    countdown.value = duration;
    status.value = "initial";
    if (countdownId) {
      uni.removeStorageSync(countdownId);
    }
  };
  const clear = () => {
    clearInterval(timer);
    status.value = "initial";
  };

  if (countdownId) {
    startTime = +(uni.getStorageSync(`countdown-${options.id}`) as string);
    const elapsed = (Date.now() - startTime) / 1000;
    if (elapsed < duration) {
      countdown.value = Math.ceil(duration - elapsed);
      status.value = "progressing";
      start();
    } else {
      uni.removeStorageSync(countdownId);
    }
  }

  return {
    countdown,
    status,
    start,
    pause,
    stop,
    reset,
    clear
  };
}
