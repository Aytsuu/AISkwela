import { EnrollmentCreateRequest } from "../types/enrollment";
import { api } from "./api.service";

export const EnrollmentService = {
  create: async (data: EnrollmentCreateRequest) => {
    try {
      const res = await api.post('api/enrollment/create', data);
      return res.data;
    } catch (err) {
      throw err;
    }
  },
  getEnrolledClasses: async (userId: string) => {
    try {
      const res = await api.get(`api/enrollment/get/${userId}`)
      return res.data;
    } catch (err) {
      throw err;
    }
  },
  updateStatus: async ({ classId, userId }: { classId: string; userId: string }) => {
    try {
      const res = await api.patch(`api/enrollment/update/status/${classId}/${userId}`);
      return res.data;
    } catch (err) {
      throw err;
    }
  }
}