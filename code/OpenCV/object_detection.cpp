#include <opencv2/opencv.hpp>

int main() {
    // Read and display an image
    
    cv::Mat image = cv::imread("/mnt/d/Academic/Semester 5/3YP/e19-3yp-First-Person-AR-Game-with-Localization/docs/images/sample.png");
    if (image.empty()) {
        std::cout << "Could not open or find the image!" << std::endl;
        return -1;
    }

    cv::imshow("Image", image);
    cv::waitKey(0);
    return 0;
}
