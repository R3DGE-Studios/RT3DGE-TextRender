# NOT A RT3DGE LIBRARY
import ctypes
import numpy as np

# Load the DLL
dll = ctypes.cdll.LoadLibrary('YourDLLName.dll')  # Replace 'YourDLLName.dll' with the actual DLL filename

# Define function argument and return types
dll.GetPixelData.argtypes = [ctypes.c_char_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_int, ctypes.c_int]
dll.GetPixelData.restype = ctypes.POINTER(ctypes.c_ubyte)

def get_pixel_data(font_file_path, text, font_size, width, height):
    # Call the C# function
    pixel_data_ptr = dll.GetPixelData(font_file_path.encode(), text.encode(), font_size, width, height)
    
    # Convert the pixel data to a NumPy array
    pixel_data = np.ctypeslib.as_array(pixel_data_ptr, shape=(width * height * 3,))
    
    # Free the memory allocated by the C# function
    ctypes.windll.kernel32.GlobalFree(pixel_data_ptr)
    
    # Reshape the pixel data to match the image dimensions
    pixel_data = pixel_data.reshape((height, width, 3))
    
    return pixel_data

# Example usage
font_file_path = "YourFontFile.ttf"
text = "Hello"
font_size = 12
width = 100
height = 100

pixel_data = get_pixel_data(font_file_path, text, font_size, width, height)
print(pixel_data)
