'use client';

import { useParams, useRouter } from "next/navigation";
import { useAuth } from "../../../components/context/AuthContext";
import { useGetClassroomData } from "../../../hooks/useClassroom";
import { Button } from "../../../components/ui/button";
import { useUpdateStatus } from "../../../hooks/useEnrollment";
import { useEffect, useState } from "react";

export default function ClassroomPage() {
  const { user } = useAuth();
  const router = useRouter();
  const params = useParams();
  const classId = params.id as string;

  const [isMounted, setIsMounted] = useState(false);

  const { mutateAsync: updateStatus } = useUpdateStatus()
  const { data: classroomData } = useGetClassroomData(classId, user?.userId, user?.role)

  // Effects
  useEffect(() => {
    setIsMounted(true);
  }, [])

  // Handlers
  const handleLeaveClass = async () => {
    try {
      await updateStatus({
        classId: classId,
        userId: user?.userId
      });
      router.back();
    } catch (err) {
      alert("Failed to update enrollment status. Please try again.");
    } finally {

    }
  }

  if (!isMounted) return null;

  return (
    <div>
      {classroomData?.class_id}
      {classroomData && (
        <Button onClick={handleLeaveClass}>Leave Classroom</Button>
      )}
    </div>
  )
}   