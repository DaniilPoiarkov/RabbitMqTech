import { Notification } from "./notification";

export interface PrivateNotification extends Notification {
    senderId: number
}
