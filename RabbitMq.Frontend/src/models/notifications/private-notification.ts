import { User } from "../user";
import { Notification } from "./notification";

export interface PrivateNotification extends Notification {
    senderId: number
    sender?: User
}
