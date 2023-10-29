#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using VRC_AvatarDescriptor = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;
using UpdraftShop.EditorWindow.Utility;

namespace UpdraftShop.EditorWindow.View
{
    public sealed class SceneObjectView : System.IDisposable
    {
        #region Constant
        private const int RendererSize = 1024;
        private const float PreviewSize = 256f;
        public  float ViewPreviewSize => PreviewSize;
        private const float PreviewMargin = 15f;

        private const float DefaultCameraOrthographicSize = 0.5f;

        private const float CameraRotateIncrement = 10;
        private const float CameraZoomIncrement = 0.01f;
        private const float CameraHeightIncrement = 0.05f;
        #endregion

        #region Variable
        // avatar
        private GameObject _targetObject;
        private VRC_AvatarDescriptor _avatarDescriptor;

        // drawing
        private readonly Camera _camera;
        private readonly Light _light;
        private readonly Scene _previewScene;
        private RenderTexture _renderTexture;
        private Texture2D _texture2D;

        // camera setting
        private float _cameraAngle;
        private float _cameraOrthographicSize;
        private float _cameraHeight;
        private Vector3 _cameraInitPosition;

        // light setting
        private readonly Quaternion DefaultLightRotation = Quaternion.Euler(50, -30, 0);

        private Color _cameraBackgroundColor = Color.gray;
        public Color CameraBackgroundColor => _cameraBackgroundColor;

        public enum DefaultCameraPosition
        {
            None,
            Face,
        }
        #endregion


        #region constructor
        public SceneObjectView()
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            // create objects
            _camera = CreateCamera();
            _renderTexture = CreateRenderTexture();
            _light = CreateLight();

            _camera.targetTexture = _renderTexture;

            ResetCameraPosition(DefaultCameraPosition.None);
            OnRender();
            Init();
        }

        public SceneObjectView(VRC_AvatarDescriptor targetObject)
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
            // create objects
            _camera = CreateCamera();
            _renderTexture = CreateRenderTexture();
            _light = CreateLight();

            _camera.targetTexture = _renderTexture;

            SetTargetObject(targetObject);
            ResetCameraPosition(DefaultCameraPosition.None);
            OnRender();
            Init();
        }
        #endregion


        private void Init()
        {
            EditorSceneManager.sceneClosed += OnSceneClosed;
        }

        private Camera CreateCamera()
        {
            var camera = EditorUtility.CreateGameObjectWithHideFlags("Object Preview Camera",
                                                           HideFlags.HideAndDontSave,
                                                           typeof(Camera)).GetComponent<Camera>();
            AddPreviewGameObject(camera.gameObject);

            camera.cameraType = CameraType.Preview;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = _cameraBackgroundColor;
            camera.orthographic = true;
            camera.orthographicSize = DefaultCameraOrthographicSize;
            camera.nearClipPlane = 0.01f;
            camera.forceIntoRenderTexture = true;
            camera.scene = _previewScene;
            camera.enabled = false;

            return camera;
        }

        private RenderTexture CreateRenderTexture()
        {
            var renderTexture = new RenderTexture(RendererSize, RendererSize, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            return renderTexture;
        }

        private Light CreateLight()
        {
            var light = EditorUtility.CreateGameObjectWithHideFlags("Object Preview Light",
                                                           HideFlags.HideAndDontSave,
                                                           typeof(Light)).GetComponent<Light>();
            AddPreviewGameObject(light.gameObject);
            light.type = LightType.Directional;
            light.gameObject.transform.rotation = DefaultLightRotation;

            return light;
        }

        public void ResetCameraPosition(DefaultCameraPosition defaultCameraPosition)
        {
            if (_camera == null)
            {
                return;
            }

            if (_targetObject != null)
            {
                _cameraAngle = 180;
                _cameraOrthographicSize = DefaultCameraOrthographicSize;
                _camera.orthographicSize = _cameraOrthographicSize;
                _cameraHeight = 0;
                _cameraInitPosition = new Vector3(0, _avatarDescriptor?.ViewPosition.y ?? 0, 0);

                MoveCameraPosition();
                if (defaultCameraPosition == DefaultCameraPosition.Face)
                {
                    ZoomCamera(-CameraZoomIncrement * 35);
                }
                OnRender();
            }
            else
            {
                _camera.transform.position = Vector3.zero;
                _camera.transform.rotation = Quaternion.Euler(Vector3.zero);
                OnRender();
            }
        }

        private void MoveCameraPosition()
        {
            _camera.transform.rotation = Quaternion.Euler(0, _cameraAngle, 0);
            var forward = _camera.transform.forward;
            var movePosition = -forward;
            movePosition.y += _cameraHeight;

            _camera.transform.position = _cameraInitPosition + movePosition;
        }

        private void ZoomCamera(float zoomIncrement)
        {
            _cameraOrthographicSize = Mathf.Max(_cameraOrthographicSize + zoomIncrement, 0.01f);
            _camera.orthographicSize = _cameraOrthographicSize;
        }

        #region SetTargetObject
        public void SetTargetObject(VRC_AvatarDescriptor avatarDescriptor, DefaultCameraPosition defaultCameraPosition = DefaultCameraPosition.None)
        {
            SetTargetObject(avatarDescriptor, avatarDescriptor != null? avatarDescriptor.gameObject : null, defaultCameraPosition: defaultCameraPosition);
        }

        public void SetTargetObject(GameObject targetObject, DefaultCameraPosition defaultCameraPosition = DefaultCameraPosition.None)
        {
            SetTargetObject(targetObject.TryGetComponent<VRC_AvatarDescriptor>(out var avatarDescriptor) ? avatarDescriptor : null, targetObject, defaultCameraPosition: defaultCameraPosition);
        }

        public void SetTargetObject(VRC_AvatarDescriptor avatarDescriptor, GameObject targetObject, DefaultCameraPosition defaultCameraPosition = DefaultCameraPosition.None)
        {
            // discard once
            Object.DestroyImmediate(_targetObject);
            _avatarDescriptor = avatarDescriptor;
            _targetObject = null;

            // clone target object
            if (targetObject != null)
            {
                var cloneTargetObject = GameObject.Instantiate(targetObject.gameObject);
                cloneTargetObject.name = targetObject.name;
                cloneTargetObject.transform.position = Vector3.zero;
                cloneTargetObject.transform.rotation = Quaternion.identity;
                cloneTargetObject.hideFlags = HideFlags.HideAndDontSave;
                cloneTargetObject.SetActive(true);

                _targetObject = cloneTargetObject;
                AddPreviewGameObject(cloneTargetObject);
            }

            ResetCameraPosition(defaultCameraPosition);
            OnRender();
        }
        #endregion

        public GameObject GetViewAvatarGameObject()
        {
            return _targetObject;
        }

        public void SetCameraBackgroundColor(Color color)
        {
            _cameraBackgroundColor = color;
            _camera.backgroundColor = _cameraBackgroundColor;
        }

        public void OnRender()
        {
            if (_camera == null || _renderTexture == null)
            {
                _texture2D = EditorGUIUtility.whiteTexture;
                return;
            }

            _camera.enabled = true;
            _camera.Render();
            _camera.enabled = false;

            // convert render texture to 2d texture.
            var width = _renderTexture.width;
            var height = _renderTexture.height;

            if (_texture2D == null || _texture2D.width != width || _texture2D.height != height)
            {
                _texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);    
            }
            RenderTexture.active = _renderTexture;
            _texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            _texture2D.Apply();
            RenderTexture.active = null;
        }

        public void ViewGUI(bool outline, float previewSizeWidth = PreviewSize, float previewSizeHeight = PreviewSize, float adjustmentX = 0.0f, float adjustmentY = 0.0f)
        {
            var reRender = false;
            var operationResultAmplificationRate = Event.current.shift ? 10 : 1;

            // draw texture
            var textureRect = GUILayoutUtility.GetRect(previewSizeWidth, previewSizeHeight, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            textureRect = new Rect(textureRect.x + PreviewMargin, textureRect.y - PreviewMargin, textureRect.width - 2 * PreviewMargin, textureRect.height - 2 * PreviewMargin);
            // adjustment position
            textureRect.x += adjustmentX;
            textureRect.y += adjustmentY;
            // preview
            if (_renderTexture == null)
            {
                _renderTexture = CreateRenderTexture();
                _camera.targetTexture = _renderTexture;
                reRender = true;
            }
            else
            {
                EditorGUI.DrawPreviewTexture(textureRect, _texture2D, null, ScaleMode.ScaleToFit);
            }
            const int ButtonMargin = 3;

            // angle button
            const string RotateLeftCharacter = "<";
            const string RotateRightCharacter = ">";
            const int RotateButtonWidth = 30;
            const int RotateButtonHeight = 15;

            if (GUI.Button(new Rect(textureRect.xMin + ButtonMargin, textureRect.yMax - RotateButtonHeight - ButtonMargin, RotateButtonWidth, RotateButtonHeight), RotateLeftCharacter))
            {
                _cameraAngle -= CameraRotateIncrement * operationResultAmplificationRate;
                MoveCameraPosition();
                reRender = true;
            }
            else if (GUI.Button(new Rect(textureRect.xMax - RotateButtonWidth - ButtonMargin, textureRect.yMax - RotateButtonHeight - ButtonMargin, RotateButtonWidth, RotateButtonHeight), RotateRightCharacter))
            {
                _cameraAngle += CameraRotateIncrement * operationResultAmplificationRate;
                MoveCameraPosition();
                reRender = true;
            }

            // zoom button
            const string ZoomUpCharacter = "+";
            const string ZoomDownCharacter = "-";
            const int ZoomButtonWidth = 17;
            const int ZoomButtonHeight = 17;

            if (GUI.Button(new Rect(textureRect.xMax - ZoomButtonWidth - ButtonMargin, textureRect.yMin + ButtonMargin, ZoomButtonWidth, ZoomButtonHeight), ZoomUpCharacter))
            {
                ZoomCamera(-CameraZoomIncrement * operationResultAmplificationRate);
                reRender = true;
            }
            else if (GUI.Button(new Rect(textureRect.xMax - ZoomButtonWidth - ButtonMargin, textureRect.yMin + ZoomButtonHeight + ButtonMargin * 2, ZoomButtonWidth, ZoomButtonHeight), ZoomDownCharacter))
            {
                ZoomCamera(CameraZoomIncrement * operationResultAmplificationRate);
                reRender = true;
            }

            // height button
            const string HeightUpCharacter = "↑";
            const string HeightDownCharacter = "↓";
            const int HeightButtonWidth = 17;
            const int HeightButtonHeight = 17;

            float heightRate = _cameraOrthographicSize / DefaultCameraOrthographicSize;

            if (GUI.Button(new Rect(textureRect.xMin + ButtonMargin, textureRect.yMin + ButtonMargin, HeightButtonWidth, HeightButtonHeight), HeightUpCharacter))
            {
                _cameraHeight += (CameraHeightIncrement * operationResultAmplificationRate) * heightRate;
                MoveCameraPosition();
                reRender = true;
            }
            else if (GUI.Button(new Rect(textureRect.xMin + ButtonMargin, textureRect.yMin + HeightButtonHeight + ButtonMargin * 2, HeightButtonWidth, HeightButtonHeight), HeightDownCharacter))
            {
                _cameraHeight -= (CameraHeightIncrement * operationResultAmplificationRate) * heightRate;
                MoveCameraPosition();
                reRender = true;
            }

            // draw outline
            if (outline)
            {
                EditorWindowUtility.DrawOutline(textureRect);
            }

            // ReDrawing
            if (reRender)
            {
                OnRender();
            }
        }

        public void AddPreviewGameObject(GameObject previewObject)
        {
            SceneManager.MoveGameObjectToScene(previewObject, _previewScene);
        }

        public void Dispose()
        {
            if (_camera != null)
            {
                _camera.targetTexture = null;
                Object.DestroyImmediate(_camera.gameObject);
            }
            if (_targetObject != null)
            {
                Object.DestroyImmediate(_targetObject);
            }
            if (_renderTexture != null)
            {
                Object.DestroyImmediate(_renderTexture);
            }
            if (_light != null)
            {
                Object.DestroyImmediate(_light.gameObject);
            }
            if (_texture2D != null)
            {
                Object.DestroyImmediate(_texture2D);
            }
            if (_previewScene.IsValid())
            {
                EditorSceneManager.CloseScene(_previewScene, true);
            }

            EditorSceneManager.sceneClosed -= OnSceneClosed;
        }

        private void OnSceneClosed(Scene scene)
        {
            VRC_AvatarDescriptor avatarDescriptor = null;
            SetTargetObject(avatarDescriptor);
        }
    }
}
#endif