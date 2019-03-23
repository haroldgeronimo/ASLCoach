using System.Collections;
using System.Collections.Generic;
using OpenCVForUnitySample;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;

namespace OpenCVModule
{
    /// <summary>
    /// Hand Pose Estimation sample
    /// referring to the https://www.youtube.com/watch?v=KuGpOxOcpds.
    /// </summary>
    [RequireComponent(typeof(WebCamTextureToMatHelper))]
    public class HandPoseAlphabetEstimation : MonoBehaviour
    {
        /// <summary>
        /// Finger count library for guessing letter
        /// </summary>
        public FingerCountLibrary FCL;
        public HandPoseDetector HPD;

        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The threashold slider.
        /// </summary>
        public Slider threasholdSlider;

        /// <summary>
        /// The BLOB color hsv.
        /// </summary>
        private Scalar blobColorHsv;

        ///// <summary>
        ///// The BLOB color rgba.
        ///// </summary>
        //private Scalar blobColorRgba;

        /// <summary>
        /// The detector.
        /// </summary>
        private ColorBlobDetector detector;

        /// <summary>
        /// The spectrum mat.
        /// </summary>
        private Mat spectrumMat;

        /// <summary>
        /// The is color selected.
        /// </summary>
        private bool isColorSelected = false;

        /// <summary>
        /// The SPECTRU m_ SIZ.
        /// </summary>
        private Size SPECTRUM_SIZE;

        /// <summary>
        /// The CONTOU r_ COLO.
        /// </summary>
        private Scalar CONTOUR_COLOR;

        /// <summary>
        /// The CONTOU r_ COLO r_ WHIT.
        /// </summary>
        private Scalar CONTOUR_COLOR_WHITE;

        /// <summary>
        /// The number of fingers.
        /// </summary>
        int numberOfFingers = 0;

        /// <summary>
        /// The web cam texture to mat helper.
        /// </summary>
        [HideInInspector] public WebCamTextureToMatHelper webCamTextureToMatHelper;

        /// <summary>
        /// The stored touch point.
        /// </summary>
        Point storedTouchPoint;

        /// <summary>
        /// Stores the RawImage where the texture is to be diplayed. 
        /// </summary>
        public RawImage camDisplay;

        public string letter = " ";

        // Use this for initialization
        void Start()
        {
            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();
            webCamTextureToMatHelper.Init();
        }

        /// <summary>
        /// Raises the web cam texture to mat helper inited event.
        /// </summary>
        public void OnWebCamTextureToMatHelperInited()
        {
            Debug.Log("OnWebCamTextureToMatHelperInited");

            //HPD.DebugWriteLn ("OnWebCamTextureToMatHelperInited");
            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();
            // HPD.DebugWriteLn ("webCamTextureToMatHelper.GetMat ()");

            texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
            // HPD.DebugWriteLn ("new Texture2D");

            //gameObject.GetComponent<Renderer> ().material.mainTexture = texture;
            if (camDisplay == null)
                camDisplay = gameObject.GetComponent<RawImage>();

            if (camDisplay == null)
                Debug.LogError("Cam display not found! Check if assigned it on inspector or the ccomponent is attached with a raw image");

            //HPD.DebugWriteLn ("camDisplay.texture");
            camDisplay.texture = texture;
            //            camDisplay.gameObject.transform.localScale = new Vector3 (webCamTextureMat.cols (), webCamTextureMat.rows (), 1);

            Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

            float width = webCamTextureMat.width();
            float height = webCamTextureMat.height();
            //HPD.DebugWriteLn ("GetTexture DImesions");

            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;

            //.DebugWriteLn ("Get Scale");
            /*     if (widthScale < heightScale) {
                    Camera.main.orthographicSize = (width * (float) Screen.height / (float) Screen.width) / 2;
                } else {
                    Camera.main.orthographicSize = height / 2;
                } */
            //HPD.DebugWriteLn ("Detector is going INIT!");
            detector = new ColorBlobDetector();
            /* if (detector == null) {
                HPD.DebugWriteLn ("Detector is null INIT!");
                return;
            } else {
                HPD.DebugWriteLn ("Detector is not null INIT!");

            } */
            spectrumMat = new Mat();
            //blobColorRgba = new Scalar (255);
            blobColorHsv = new Scalar(255);
            SPECTRUM_SIZE = new Size(200, 64);
            CONTOUR_COLOR = new Scalar(0, 255, 0, 255);
            CONTOUR_COLOR_WHITE = new Scalar(255, 255, 255, 255);
        }

        /// <summary>
        /// Raises the web cam texture to mat helper disposed event.
        /// </summary>
        public void OnWebCamTextureToMatHelperDisposed()
        {
            Debug.Log("OnWebCamTextureToMatHelperDisposed");

            if (spectrumMat != null)
                spectrumMat.Dispose();
        }

        /// <summary>
        /// Raises the web cam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }

        // Update is called once per frame
        void Update()
        {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
            //Touch
            int touchCount = Input.touchCount;
            if (touchCount == 1) {
                Touch t = Input.GetTouch (0);
                if (t.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject (t.fingerId)) {
                    storedTouchPoint = new Point (t.position.x, t.position.y);
                    //Debug.Log ("touch X " + t.position.x);
                    //Debug.Log ("touch Y " + t.position.y);
                }
            }
#else
            //Mouse
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                storedTouchPoint = new Point(Input.mousePosition.x, Input.mousePosition.y);
                //Debug.Log ("mouse X " + Input.mousePosition.x);
                //Debug.Log ("mouse Y " + Input.mousePosition.y);
            }
#endif

            if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
            {

                Mat rgbaMat = webCamTextureToMatHelper.GetMat();

                if (storedTouchPoint != null)
                {
                    OnCenterTouch(rgbaMat);
                    storedTouchPoint = null;
                }

                //handPoseEstimationProcessNew(rgbaMat);
                handPoseEstimationProcess (rgbaMat);

                //Imgproc.putText (rgbaMat, "Please touch the area of the open hand.", new Point (5, rgbaMat.rows () - 10), Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

                //              Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

                Utils.matToTexture2D(rgbaMat, texture, webCamTextureToMatHelper.GetBufferColors());
            }
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        void OnDisable()
        {
            webCamTextureToMatHelper.Dispose();
        }

        /// <summary>
        /// Raises the play button event.
        /// </summary>
        public void OnPlayButton()
        {
            webCamTextureToMatHelper.Play();
        }

        /// <summary>
        /// Raises the pause button event.
        /// </summary>
        public void OnPauseButton()
        {
            webCamTextureToMatHelper.Pause();
        }

        /// <summary>
        /// Raises the stop button event.
        /// </summary>
        public void OnStopButton()
        {
            webCamTextureToMatHelper.Stop();
        }

        /// <summary>
        /// Raises the change camera button event.
        /// </summary>
        public void OnChangeCameraButton()
        {
            webCamTextureToMatHelper.Init(null, webCamTextureToMatHelper.requestWidth, webCamTextureToMatHelper.requestHeight, !webCamTextureToMatHelper.requestIsFrontFacing);
        }

        /// <summary>
        /// Hands the pose estimation process.
        /// </summary>
        public void handPoseEstimationProcess(Mat rgbaMat)
        {

            //Imgproc.blur(mRgba, mRgba, new Size(5,5));
            Imgproc.GaussianBlur(rgbaMat, rgbaMat, new OpenCVForUnity.Size(3, 3), 1, 1);
            //Imgproc.medianBlur(mRgba, mRgba, 3);

            if (!isColorSelected)
                return;

            List<MatOfPoint> contours = detector.getContours();
            detector.process(rgbaMat);

            //                      Debug.Log ("Contours count: " + contours.Count);

            if (contours.Count <= 0)
            {
                return;
            }

            RotatedRect rect = Imgproc.minAreaRect(new MatOfPoint2f(contours[0].toArray()));

            double boundWidth = rect.size.width;
            double boundHeight = rect.size.height;
            int boundPos = 0;

            for (int i = 1; i < contours.Count; i++)
            {
                rect = Imgproc.minAreaRect(new MatOfPoint2f(contours[i].toArray()));
                if (rect.size.width * rect.size.height > boundWidth * boundHeight)
                {
                    boundWidth = rect.size.width;
                    boundHeight = rect.size.height;
                    boundPos = i;
                }
            }

            OpenCVForUnity.Rect boundRect = Imgproc.boundingRect(new MatOfPoint(contours[boundPos].toArray()));
            Imgproc.rectangle(rgbaMat, boundRect.tl(), boundRect.br(), CONTOUR_COLOR_WHITE, 2, 8, 0);

            //                      Debug.Log (
            //                      " Row start [" + 
            //                              (int)boundRect.tl ().y + "] row end [" +
            //                              (int)boundRect.br ().y + "] Col start [" +
            //                              (int)boundRect.tl ().x + "] Col end [" +
            //                              (int)boundRect.br ().x + "]");

            double a = boundRect.br().y - boundRect.tl().y;
            a = a * 0.7;
            a = boundRect.tl().y + a;

            //                      Debug.Log (
            //                      " A [" + a + "] br y - tl y = [" + (boundRect.br ().y - boundRect.tl ().y) + "]");

            //Core.rectangle( mRgba, boundRect.tl(), boundRect.br(), CONTOUR_COLOR, 2, 8, 0 );
            Imgproc.rectangle(rgbaMat, boundRect.tl(), new Point(boundRect.br().x, a), CONTOUR_COLOR, 2, 8, 0);

            MatOfPoint2f pointMat = new MatOfPoint2f();
            Imgproc.approxPolyDP(new MatOfPoint2f(contours[boundPos].toArray()), pointMat, 3, true);
            contours[boundPos] = new MatOfPoint(pointMat.toArray());

            MatOfInt hull = new MatOfInt();
            MatOfInt4 convexDefect = new MatOfInt4();
            Imgproc.convexHull(new MatOfPoint(contours[boundPos].toArray()), hull);

            if (hull.toArray().Length < 3)
                return;

            Imgproc.convexityDefects(new MatOfPoint(contours[boundPos].toArray()), hull, convexDefect);

            List<MatOfPoint> hullPoints = new List<MatOfPoint>();
            List<Point> listPo = new List<Point>();
            for (int j = 0; j < hull.toList().Count; j++)
            {
                listPo.Add(contours[boundPos].toList()[hull.toList()[j]]);
            }

            MatOfPoint e = new MatOfPoint();
            e.fromList(listPo);
            hullPoints.Add(e);

            List<MatOfPoint> defectPoints = new List<MatOfPoint>();
            List<Point> listPoDefect = new List<Point>();
            for (int j = 0; j < convexDefect.toList().Count; j = j + 4)
            {
                Point farPoint = contours[boundPos].toList()[convexDefect.toList()[j + 2]];
                int depth = convexDefect.toList()[j + 3];
                if (depth > threasholdSlider.value && farPoint.y < a)
                {
                    listPoDefect.Add(contours[boundPos].toList()[convexDefect.toList()[j + 2]]);
                }
                //                              Debug.Log ("defects [" + j + "] " + convexDefect.toList () [j + 3]);
            }

            MatOfPoint e2 = new MatOfPoint();
            e2.fromList(listPo);
            defectPoints.Add(e2);

            //                      Debug.Log ("hull: " + hull.toList ());
            //                      Debug.Log ("defects: " + convexDefect.toList ());

            Imgproc.drawContours(rgbaMat, hullPoints, -1, CONTOUR_COLOR, 3);

            //                      int defectsTotal = (int)convexDefect.total();
            //                      Debug.Log ("Defect total " + defectsTotal);

            this.numberOfFingers = listPoDefect.Count;
            if (this.numberOfFingers > 5)
                this.numberOfFingers = 5;
            letter = FCL.GuessLetter(numberOfFingers).ToString();
            //                      Debug.Log ("numberOfFingers " + numberOfFingers);

            //                      Core.putText (mRgba, "" + numberOfFingers, new Point (mRgba.cols () / 2, mRgba.rows () / 2), Core.FONT_HERSHEY_PLAIN, 4.0, new Scalar (255, 255, 255, 255), 6, Core.LINE_AA, false);
            // numberOfFingersText.text = letter;

            foreach (Point p in listPoDefect)
            {
                Imgproc.circle(rgbaMat, p, 6, new Scalar(255, 0, 255, 255), -1);
            }
        }
        #region  commented
        /* 
                /// <summary>
                /// Ons the touch.
                /// </summary>
                /// <param name="touchPoint">Touch point.</param>
                public void onTouch (Mat rgbaMat, Point touchPoint) {
                    int cols = rgbaMat.cols ();
                    int rows = rgbaMat.rows ();

                    int x = (int) touchPoint.x;
                    int y = (int) touchPoint.y;

                    //                      Debug.Log ("Touch image coordinates: (" + x + ", " + y + ")");

                    if ((x < 0) || (y < 0) || (x > cols) || (y > rows))
                        return;

                    OpenCVForUnity.Rect touchedRect = new OpenCVForUnity.Rect ();

                    touchedRect.x = (x > 5) ? x - 5 : 0;
                    touchedRect.y = (y > 5) ? y - 5 : 0;

                    touchedRect.width = (x + 5 < cols) ? x + 5 - touchedRect.x : cols - touchedRect.x;
                    touchedRect.height = (y + 5 < rows) ? y + 5 - touchedRect.y : rows - touchedRect.y;

                    Mat touchedRegionRgba = rgbaMat.submat (touchedRect);

                    Mat touchedRegionHsv = new Mat ();
                    Imgproc.cvtColor (touchedRegionRgba, touchedRegionHsv, Imgproc.COLOR_RGB2HSV_FULL);

                    // Calculate average color of touched region
                    blobColorHsv = Core.sumElems (touchedRegionHsv);
                    int pointCount = touchedRect.width * touchedRect.height;
                    for (int i = 0; i < blobColorHsv.val.Length; i++)
                        blobColorHsv.val[i] /= pointCount;

                    //blobColorRgba = converScalarHsv2Rgba (blobColorHsv);

                    //                      Debug.Log ("Touched rgba color: (" + mBlobColorRgba.val [0] + ", " + mBlobColorRgba.val [1] +
                    //                              ", " + mBlobColorRgba.val [2] + ", " + mBlobColorRgba.val [3] + ")");

                    detector.setHsvColor (blobColorHsv);

                    Imgproc.resize (detector.getSpectrum (), spectrumMat, SPECTRUM_SIZE);

                    isColorSelected = true;

                    touchedRegionRgba.release ();
                    touchedRegionHsv.release ();
                }

                /// <summary>
                /// Convers the scalar hsv2 rgba.
                /// </summary>
                /// <returns>The scalar hsv2 rgba.</returns>
                /// <param name="hsvColor">Hsv color.</param>
                private Scalar converScalarHsv2Rgba (Scalar hsvColor) {
                    Mat pointMatRgba = new Mat ();
                    Mat pointMatHsv = new Mat (1, 1, CvType.CV_8UC3, hsvColor);
                    Imgproc.cvtColor (pointMatHsv, pointMatRgba, Imgproc.COLOR_HSV2RGB_FULL, 4);

                    return new Scalar (pointMatRgba.get (0, 0));
                } */
        #endregion
        public void OnCenterTouch(Mat rgbaMat)
        {
            int cols = rgbaMat.cols();
            int rows = rgbaMat.rows();

            int x = (int)cols / 2;
            int y = (int)rows / 2;

            //                      Debug.Log ("Touch image coordinates: (" + x + ", " + y + ")");

            if ((x < 0) || (y < 0) || (x > cols) || (y > rows))
                return;

            OpenCVForUnity.Rect touchedRect = new OpenCVForUnity.Rect();

            touchedRect.x = (x > 5) ? x - 5 : 0;
            touchedRect.y = (y > 5) ? y - 5 : 0;

            touchedRect.width = (x + 5 < cols) ? x + 5 - touchedRect.x : cols - touchedRect.x;
            touchedRect.height = (y + 5 < rows) ? y + 5 - touchedRect.y : rows - touchedRect.y;

            Mat touchedRegionRgba = rgbaMat.submat(touchedRect);

            Mat touchedRegionHsv = new Mat();
            Imgproc.cvtColor(touchedRegionRgba, touchedRegionHsv, Imgproc.COLOR_RGB2HSV_FULL);
            //return;
            // Calculate average color of touched region
            blobColorHsv = Core.sumElems(touchedRegionHsv);
            int pointCount = touchedRect.width * touchedRect.height;
            for (int i = 0; i < blobColorHsv.val.Length; i++)
                blobColorHsv.val[i] /= pointCount;

            //blobColorRgba = converScalarHsv2Rgba (blobColorHsv);

            //                      Debug.Log ("Touched rgba color: (" + mBlobColorRgba.val [0] + ", " + mBlobColorRgba.val [1] +
            //                              ", " + mBlobColorRgba.val [2] + ", " + mBlobColorRgba.val [3] + ")");
            //return;
            /* if (detector != null) {
                numberOfFingersText.text = "Detector not null";
                return;
            } else {
                numberOfFingersText.text = "Detector is null";
                return;
            } */

            if (detector == null)
            {
                HPD.DebugWriteLn("Detector is null!");
                return;
            }
            detector.setHsvColor(blobColorHsv);

            //    return;

            Imgproc.resize(detector.getSpectrum(), spectrumMat, SPECTRUM_SIZE);

            isColorSelected = true;

            touchedRegionRgba.release();
            touchedRegionHsv.release();
        }

        /// <summary>
        /// Converts the screen point.
        /// </summary>
        /// <returns>The screen point.</returns>
        /// <param name="screenPoint">Screen point.</param>
        /// <param name="quad">Quad.</param>
        /// <param name="cam">Cam.</param>
        static Point convertScreenPoint(Point screenPoint, GameObject quad, Camera cam)
        {
            Vector2 tl;
            Vector2 tr;
            Vector2 br;
            Vector2 bl;

            tl = cam.WorldToScreenPoint(new Vector3(quad.transform.localPosition.x - quad.transform.localScale.x / 2, quad.transform.localPosition.y + quad.transform.localScale.y / 2, quad.transform.localPosition.z));
            tr = cam.WorldToScreenPoint(new Vector3(quad.transform.localPosition.x + quad.transform.localScale.x / 2, quad.transform.localPosition.y + quad.transform.localScale.y / 2, quad.transform.localPosition.z));
            br = cam.WorldToScreenPoint(new Vector3(quad.transform.localPosition.x + quad.transform.localScale.x / 2, quad.transform.localPosition.y - quad.transform.localScale.y / 2, quad.transform.localPosition.z));
            bl = cam.WorldToScreenPoint(new Vector3(quad.transform.localPosition.x - quad.transform.localScale.x / 2, quad.transform.localPosition.y - quad.transform.localScale.y / 2, quad.transform.localPosition.z));

            Mat srcRectMat = new Mat(4, 1, CvType.CV_32FC2);
            Mat dstRectMat = new Mat(4, 1, CvType.CV_32FC2);

            srcRectMat.put(0, 0, tl.x, tl.y, tr.x, tr.y, br.x, br.y, bl.x, bl.y);
            dstRectMat.put(0, 0, 0.0, 0.0, quad.transform.localScale.x, 0.0, quad.transform.localScale.x, quad.transform.localScale.y, 0.0, quad.transform.localScale.y);

            Mat perspectiveTransform = Imgproc.getPerspectiveTransform(srcRectMat, dstRectMat);

            //                      Debug.Log ("srcRectMat " + srcRectMat.dump ());
            //                      Debug.Log ("dstRectMat " + dstRectMat.dump ());
            //                      Debug.Log ("perspectiveTransform " + perspectiveTransform.dump ());

            MatOfPoint2f srcPointMat = new MatOfPoint2f(screenPoint);
            MatOfPoint2f dstPointMat = new MatOfPoint2f();

            Core.perspectiveTransform(srcPointMat, dstPointMat, perspectiveTransform);

            //                      Debug.Log ("srcPointMat " + srcPointMat.dump ());
            //                      Debug.Log ("dstPointMat " + dstPointMat.dump ());

            return dstPointMat.toArray()[0];
        }
        double lower_thresh1 = 129;
        double upper_thresh1 = 255;
        public void handPoseEstimationProcessNew(Mat rgbaMat)
        {
            HPD.DebugClear();

            Mat gray = new Mat();
            Imgproc.cvtColor(rgbaMat, gray, Imgproc.COLOR_RGB2GRAY);
            HPD.DebugWriteLn("gray");

            Mat hsv = new Mat();
            Imgproc.cvtColor(rgbaMat, hsv, Imgproc.COLOR_RGB2HSV);
            HPD.DebugWriteLn("hsv");

            /*     int[] lower_red = new int[]{0,150,50};
            int[] upper_red = new int[]{195,255,255};
 */

            Mat blurred = new Mat();
            Imgproc.GaussianBlur(gray, blurred, new OpenCVForUnity.Size(35, 35), 1, 1);
            detector.process(rgbaMat);
            HPD.DebugWriteLn("blurred");

            Mat tresh1 = new Mat();
            Imgproc.threshold(blurred, tresh1, lower_thresh1, upper_thresh1, Imgproc.THRESH_BINARY_INV + Imgproc.THRESH_OTSU);
            HPD.DebugWriteLn("tresh1");

            Mat tresh_dofh = new Mat();
            Imgproc.threshold(rgbaMat, tresh_dofh, lower_thresh1, upper_thresh1, Imgproc.THRESH_BINARY_INV + Imgproc.THRESH_OTSU);
            HPD.DebugWriteLn("tresh_dofh");
            /* if (!isColorSelected)
                return; */

            List<MatOfPoint> contours = new List<MatOfPoint>();
            Mat srcHierarchy = new Mat();
            Imgproc.findContours(tresh1, contours, srcHierarchy, Imgproc.RETR_TREE, Imgproc.CHAIN_APPROX_NONE);
            HPD.DebugWriteLn("findContours1");
            List<MatOfPoint> xyz, hierarchy;
               HPD.DebugWriteLn("hierarchy");
            xyz = hierarchy = new List<MatOfPoint>(contours);
               HPD.DebugWriteLn("List");

            List<MatOfPoint> contours_dofh = new List<MatOfPoint>();
               HPD.DebugWriteLn("contours_dofh");
            Mat srcHierarchy_dofh = new Mat();
               HPD.DebugWriteLn("srcHierarchy_dofh");
            Imgproc.findContours(tresh_dofh, contours_dofh, srcHierarchy_dofh, 2, 1);
            HPD.DebugWriteLn("findContours2");
            List<MatOfPoint> xyz_dofh, hierarchy_dofh = new List<MatOfPoint>(contours_dofh);
            MatOfPoint cnt = new MatOfPoint();
            foreach (MatOfPoint contour in contours)
            {
                double _cnt = 0;
                double currentContour = Imgproc.contourArea(contour);
                if (currentContour > _cnt)
                {
                    cnt = contour;
                }
            }
            HPD.DebugWriteLn("loop contour");
            double area_of_contour = Imgproc.contourArea(cnt);
            OpenCVForUnity.Rect rect = new OpenCVForUnity.Rect();
            double x, y, w, h;

            rect = Imgproc.boundingRect(cnt);
            HPD.DebugWriteLn("boundingRect");
            x = rect.x;
            y = rect.y;
            w = rect.width;
            h = rect.height;
            Imgproc.rectangle(rgbaMat, new Point(x, y), new Point(x + w, y + h), new Scalar(0, 0, 255));
            MatOfInt hull = new MatOfInt();
            Imgproc.convexHull(cnt, hull);

            Imgproc.drawContours(rgbaMat, new List<MatOfPoint>() { cnt }, 0, new Scalar(0, 255, 0), 0);

            Imgproc.convexHull(cnt, hull, false);
            MatOfInt4 defects = new MatOfInt4();
            Imgproc.convexityDefects(cnt, hull, defects);

            int count_defects = 0;
            Imgproc.drawContours(tresh1, contours, -1, new Scalar(0, 255, 0), 3);

            List<Point> cntList = cnt.toList();

            List<int> defectsList = defects.toList();
            for (int i = 0; i < defectsList.Count; i++)
            {
                int s, e, f, d;
                s = e = f = d = defectsList[i];
                Vector2 start = new Vector2((float)cntList[s].x, (float)cntList[s].y);
                Vector2 end = new Vector2((float)cntList[e].x, (float)cntList[e].y);
                Vector2 far = new Vector2((float)cntList[f].x, (float)cntList[f].y);

                float a = Mathf.Sqrt(Mathf.Pow((start.x - start.x), 2) + Mathf.Pow((start.y - start.y), 2));
                float b = Mathf.Sqrt(Mathf.Pow((far.x - start.x), 2) + Mathf.Pow((far.y - start.y), 2));
                float c = Mathf.Sqrt(Mathf.Pow((end.x - far.x), 2) + Mathf.Pow((end.y - far.y), 2));

                float defect_angle = Mathf.Acos((Mathf.Pow(b, 2) + Mathf.Pow(c, 2) - Mathf.Pow(a, 2)) / (2 * b * c)) * 60;
                Imgproc.circle(rgbaMat, new Point((double)far.x, (double)far.y), 4, new Scalar(0, 0, 255), -1);

                if (defect_angle <= 90)
                    count_defects += 1;

                Imgproc.line(rgbaMat, new Point((double)start.x, (double)start.y), new Point((double)end.x, (double)end.y), new Scalar(0, 255, 0), 3);

            }

            Moments moment = Imgproc.moments(cnt);
            MatOfPoint2f cnt2f = new MatOfPoint2f(cnt);
            double perimeter = Imgproc.arcLength(cnt2f, true);
            double area = Imgproc.contourArea(cnt);

            Point center = new Point();
            float[] rad = new float[1] { 0 };
            Imgproc.minEnclosingCircle(cnt2f, center, rad);
            int radius = (int)rad[0];
            Imgproc.circle(rgbaMat, center, radius, new Scalar(255, 0, 0), 2);
            float area_of_circle = Mathf.PI * radius * radius;

            MatOfInt hull_test = new MatOfInt();
            Imgproc.convexHull(cnt, hull_test);
            double hull_area = Imgproc.contourArea(hull_test);
            float solidity = (float)area / (float)hull_area;

            double aspect_ratio = w / h;
            double rect_area = w * h;
            double extent = area / rect_area;
            RotatedRect angle_t = Imgproc.fitEllipse(cnt2f);
            double angle = angle_t.angle;

            if (area_of_circle - area < 5000)
            {
                letter = "A";
                // numberOfFingersText.text = letter;
            }
            else if (count_defects == 1)
            {
                if (angle < 10)
                    letter = "V";
                else if (angle > 40 && angle < 66)
                    letter = "C";
                else if (angle > 20 && angle < 35)
                    letter = "L";
                else
                    letter = "Y";
            }
            else if (count_defects == 2)
            {
                if (angle > 100)
                    letter = "F";
                else
                    letter = "W";
            }
            else if (count_defects == 4)
            {
                letter = "A";
            }
            else
            {
                if (area > 12000)
                    letter = "B";
                else
                {
                    if (solidity < 0.85)
                    {
                        if (aspect_ratio < 1)
                        {
                            if (angle < 20)
                                letter = "D";
                            else if (angle < 180 && angle > 169)
                                letter = "I";
                            else if (angle < 168)
                                letter = "J";
                        }
                        else if (aspect_ratio > 1.01)
                            letter = "Y";
                    }
                    else
                    {
                        if (angle > 30 && angle < 100)
                            letter = "H";
                        else if (angle > 120)
                            letter = "I";
                        else
                            letter = "U";
                    }
                }
            }

            string status = "";
            status += "\nRadius:" + (Mathf.PI * radius * radius).ToString() + " Area:" + area;
            status += "\neffective circle area:" + (area_of_circle - area).ToString();
            status += "\nsolidity:" + solidity;
            status += "\naspect ratio:" + aspect_ratio;
            status += "\ncovexity defects:" + count_defects;
            status += "\nextent:" + extent;
            status += "\nangle:" + angle;
            status += "\n Letter:" + letter;

            HPD.DebugClear();
            HPD.DebugWrite(status);
            /* 
                        // Imgproc.drawContours(rgbaMat,new List<MatOfInt>(){hull},0,new Scalar(0,255,255),0);
                        detector.process (rgbaMat);

                        //                      Debug.Log ("Contours count: " + contours.Count);

                        RotatedRect rect = Imgproc.minAreaRect (new MatOfPoint2f (contours[0].toArray ()));

                        double boundWidth = rect.size.width;
                        double boundHeight = rect.size.height;
                        int boundPos = 0;

                        for (int i = 1; i < contours.Count; i++) {
                            rect = Imgproc.minAreaRect (new MatOfPoint2f (contours[i].toArray ()));
                            if (rect.size.width * rect.size.height > boundWidth * boundHeight) {
                                boundWidth = rect.size.width;
                                boundHeight = rect.size.height;
                                boundPos = i;
                            }
                        }

                        OpenCVForUnity.Rect boundRect = Imgproc.boundingRect (new MatOfPoint (contours[boundPos].toArray ()));
                        Imgproc.rectangle (rgbaMat, boundRect.tl (), boundRect.br (), CONTOUR_COLOR_WHITE, 2, 8, 0);

                        //                      Debug.Log (
                        //                      " Row start [" + 
                        //                              (int)boundRect.tl ().y + "] row end [" +
                        //                              (int)boundRect.br ().y + "] Col start [" +
                        //                              (int)boundRect.tl ().x + "] Col end [" +
                        //                              (int)boundRect.br ().x + "]");

                        double a = boundRect.br ().y - boundRect.tl ().y;
                        a = a * 0.7;
                        a = boundRect.tl ().y + a;

                        //                      Debug.Log (
                        //                      " A [" + a + "] br y - tl y = [" + (boundRect.br ().y - boundRect.tl ().y) + "]");

                        //Core.rectangle( mRgba, boundRect.tl(), boundRect.br(), CONTOUR_COLOR, 2, 8, 0 );
                        Imgproc.rectangle (rgbaMat, boundRect.tl (), new Point (boundRect.br ().x, a), CONTOUR_COLOR, 2, 8, 0);

                        MatOfPoint2f pointMat = new MatOfPoint2f ();
                        Imgproc.approxPolyDP (new MatOfPoint2f (contours[boundPos].toArray ()), pointMat, 3, true);
                        contours[boundPos] = new MatOfPoint (pointMat.toArray ());

                        //MatOfInt hull = new MatOfInt ();
                        MatOfInt4 convexDefect = new MatOfInt4 ();
                        Imgproc.convexHull (new MatOfPoint (contours[boundPos].toArray ()), hull);

                        if (hull.toArray ().Length < 3)
                            return;

                        Imgproc.convexityDefects (new MatOfPoint (contours[boundPos].toArray ()), hull, convexDefect);

                        List<MatOfPoint> hullPoints = new List<MatOfPoint> ();
                        List<Point> listPo = new List<Point> ();
                        for (int j = 0; j < hull.toList ().Count; j++) {
                            listPo.Add (contours[boundPos].toList () [hull.toList () [j]]);
                        }

                        MatOfPoint e = new MatOfPoint ();
                        e.fromList (listPo);
                        hullPoints.Add (e);

                        List<MatOfPoint> defectPoints = new List<MatOfPoint> ();
                        List<Point> listPoDefect = new List<Point> ();
                        for (int j = 0; j < convexDefect.toList ().Count; j = j + 4) {
                            Point farPoint = contours[boundPos].toList () [convexDefect.toList () [j + 2]];
                            int depth = convexDefect.toList () [j + 3];
                            if (depth > threasholdSlider.value && farPoint.y < a) {
                                listPoDefect.Add (contours[boundPos].toList () [convexDefect.toList () [j + 2]]);
                            }
                            //                              Debug.Log ("defects [" + j + "] " + convexDefect.toList () [j + 3]);
                        }

                        MatOfPoint e2 = new MatOfPoint ();
                        e2.fromList (listPo);
                        defectPoints.Add (e2);

                        //                      Debug.Log ("hull: " + hull.toList ());
                        //                      Debug.Log ("defects: " + convexDefect.toList ());

                        Imgproc.drawContours (rgbaMat, hullPoints, -1, CONTOUR_COLOR, 3);

                        //                      int defectsTotal = (int)convexDefect.total();
                        //                      Debug.Log ("Defect total " + defectsTotal);

                        this.numberOfFingers = listPoDefect.Count;
                        if (this.numberOfFingers > 5)
                            this.numberOfFingers = 5;
                        letter = FCL.GuessLetter (numberOfFingers).ToString ();
                        //                      Debug.Log ("numberOfFingers " + numberOfFingers);

                        //                      Core.putText (mRgba, "" + numberOfFingers, new Point (mRgba.cols () / 2, mRgba.rows () / 2), Core.FONT_HERSHEY_PLAIN, 4.0, new Scalar (255, 255, 255, 255), 6, Core.LINE_AA, false);
                        // numberOfFingersText.text = letter;

                        foreach (Point p in listPoDefect) {
                            Imgproc.circle (rgbaMat, p, 6, new Scalar (255, 0, 255, 255), -1);
                        } */
        }
    }
}