// Copyright eeGeo Ltd (2012-2014), All Rights Reserved

namespace Wrld.MapInput.Mouse
<<<<<<< HEAD
{
    public class MouseTouchGesture
    {
        private IUnityInputHandler m_handler;

        private AppInterface.TouchData CreateTouchEventData(MouseInputEvent inputEvent)
        {
            var result = new AppInterface.TouchData();
            result.point.Set(inputEvent.x, inputEvent.y);
            return result;
        }

        public MouseTouchGesture(IUnityInputHandler handler)
        {
            m_handler = handler;
        }

        public void PointerDown(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchDown(CreateTouchEventData(inputEvent));
        }

        public void PointerUp(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchUp(CreateTouchEventData(inputEvent));
        }

        public void PointerMove(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchMove(CreateTouchEventData(inputEvent));
        }
    };
=======
{
    public class MouseTouchGesture
    {
        private IUnityInputHandler m_handler;

        private AppInterface.TouchData CreateTouchEventData(MouseInputEvent inputEvent)
        {
            var result = new AppInterface.TouchData();
            result.point.Set(inputEvent.x, inputEvent.y);
            return result;
        }

        public MouseTouchGesture(IUnityInputHandler handler)
        {
            m_handler = handler;
        }

        public void PointerDown(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchDown(CreateTouchEventData(inputEvent));
        }

        public void PointerUp(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchUp(CreateTouchEventData(inputEvent));
        }

        public void PointerMove(MouseInputEvent inputEvent)
        {
            m_handler.Event_TouchMove(CreateTouchEventData(inputEvent));
        }
    };
>>>>>>> 93976baab53246a27158b03be0d07d7b8897ef5e
}
