//using System;
//using System.IO;
//using System.IO.Compression;
//using OpenCvSharp;

//namespace Oh1
//{
//    public class GenerateImage
//    {
//        public void Run()
//        {
//            // 원본 이미지 경로 (필요시 경로 수정)
//            string imagePath = "Image.bmp";
//            Mat orig = Cv2.ImRead(imagePath, ImreadModes.Unchanged);
//            if (orig.Empty())
//            {
//                Console.WriteLine("이미지를 찾을 수 없습니다.");
//                return;
//            }

//            int width = orig.Width;
//            int height = orig.Height;
//            Point2f center = new Point2f(width / 2.0f, height / 2.0f);
//            Random rnd = new Random();

//            // 결과 이미지 저장 폴더 생성
//            string outputDir = "augmented_images";
//            if (!Directory.Exists(outputDir))
//                Directory.CreateDirectory(outputDir);

//            for (int i = 0; i < 100; i++)
//            {
//                // 각 변환(affine(회전), 좌표 이동, 밝기 조절) 적용 여부를 랜덤으로 결정
//                bool applyAffine = rnd.Next(2) == 1;       // 회전 적용 여부
//                bool applyTranslation = rnd.Next(2) == 1;  // 좌표 이동 적용 여부
//                bool applyBrightness = rnd.Next(2) == 1;   // 밝기 조절 적용 여부

//                // 최소 한 개의 변환은 반드시 적용되도록 보장
//                if (!applyAffine && !applyTranslation && !applyBrightness)
//                {
//                    int forcedChoice = rnd.Next(3);
//                    if (forcedChoice == 0)
//                        applyAffine = true;
//                    else if (forcedChoice == 1)
//                        applyTranslation = true;
//                    else
//                        applyBrightness = true;
//                }

//                // 각 변환에 필요한 랜덤 값 결정
//                double angle = applyAffine ? rnd.NextDouble() * 10 - 5 : 0.0; // -5 ~ +5 도
//                double dx = applyTranslation ? rnd.Next(-5, 6) : 0.0;         // -5 ~ +5 픽셀
//                double dy = applyTranslation ? rnd.Next(-5, 6) : 0.0;
//                double brightness = applyBrightness ? rnd.NextDouble() * 0.2 + 0.9 : 1.0; // 0.9 ~ 1.1

//                Mat transformed = new Mat();

//                // 회전 및 좌표 이동 중 하나라도 적용되면 affine 변환 수행
//                if (applyAffine || applyTranslation)
//                {
//                    // 회전 행렬 생성 (회전 각도와 스케일 1)
//                    Mat rotMat = Cv2.GetRotationMatrix2D(center, angle, 1.0);

//                    // 행렬의 이동 요소에 좌표 이동 값 추가
//                    rotMat.Set(0, 2, rotMat.At<double>(0, 2) + dx);
//                    rotMat.Set(1, 2, rotMat.At<double>(1, 2) + dy);

//                    // affine 변환 적용 (검은색 배경 유지)
//                    Cv2.WarpAffine(
//                        orig,
//                        transformed,
//                        rotMat,
//                        new Size(width, height),
//                        InterpolationFlags.Linear,
//                        BorderTypes.Constant,
//                        Scalar.Black
//                    );
//                }
//                else
//                {
//                    // 변환이 없으면 원본 이미지 사용
//                    transformed = orig.Clone();
//                }

//                Mat finalImage = new Mat();

//                // 밝기 조절 적용 (밝기 계수가 1이 아니면)
//                if (applyBrightness && Math.Abs(brightness - 1.0) > 1e-6)
//                {
//                    // 각 픽셀에 brightness factor를 곱함 (beta=0)
//                    transformed.ConvertTo(finalImage, -1, brightness, 0);
//                }
//                else
//                {
//                    finalImage = transformed.Clone();
//                }

//                // 결과 이미지 BMP 형식으로 저장
//                string outPath = Path.Combine(outputDir, $"augmented_{i:03d}.bmp");
//                Cv2.ImWrite(outPath, finalImage);
//            }

//            // 생성된 이미지들을 하나의 zip 파일로 압축 (폴더 전체 압축)
//            string zipPath = "augmented_images.zip";
//            if (File.Exists(zipPath))
//                File.Delete(zipPath);

//            ZipFile.CreateFromDirectory(outputDir, zipPath);

//            Console.WriteLine("데이터셋 생성 완료. Zip 파일: " + zipPath);
//        }
//    }
//}