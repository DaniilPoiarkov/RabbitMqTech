import { User } from "../user";

export interface Notification {
    id: number,
    createdAt: Date,
    recieverId: number,
    user: User,
    recieverConnectionId: string,
    content: string,
}
